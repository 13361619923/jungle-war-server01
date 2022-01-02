using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;
namespace GameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        /// <summary>
        /// 在controllerDict字典中添加固定的内容
        /// </summary>
        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到[" + requestCode + "]所对应的Controller,无法处理请求"); return;
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);

            MethodInfo mi = controller.GetType().GetMethod(methodName);//通过反射获取对应Controller中的方法
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller[" + controller.GetType() + "]中没有对应的处理方法:[" + methodName + "]"); return;
            }
            object[] parameters = new object[] { data, client, server };//将参数转化成object数组
            object o = mi.Invoke(controller, parameters);//调用Controller中的处理数据的方法
            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            Console.WriteLine("完成数据处理，发送相应");
            Console.WriteLine("响应的结果是：" + (o as string));
            //处理完数据之后发送相应
            server.SendResponse(client, actionCode, o as string);
        }

    }
}
