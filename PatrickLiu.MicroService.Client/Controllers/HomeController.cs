using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatrickLiu.MicroService.Interfaces;
using PatrickLiu.MicroService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatrickLiu.MicroService.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private static int index;

        /// <summary>
        /// 初始化该类型的新实例。
        /// </summary>
        /// <param name="logger">注入日志对象。</param>
        /// <param name="userService">注入用户服务对象。</param>
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// 首页。
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            #region 1、单体架构

            //this.ViewBag.Users = _userService.UserAll();

            #endregion

            #region 2、分布式架构

            #region 2.1、直接访问服务实例

            //string url = string.Empty;
            //url = "http://localhost:5726/api/users/all";
            //url = "http://localhost:5726/api/users/all";
            //url = "http://localhost:5726/api/users/all";

            #endregion

            #region 通过 Ngnix网关访问服务实例，默认轮训。

            string url = "http://localhost:8080/api/users/all";

            #endregion

            string content = InvokeAPI(url);
            this.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            Console.WriteLine($"This is {url} Invoke.");

            #endregion

            return View();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string InvokeAPI(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = client.SendAsync(message).Result;
                string conent = result.Content.ReadAsStringAsync().Result;
                return conent;
            }
        }
    }
}
