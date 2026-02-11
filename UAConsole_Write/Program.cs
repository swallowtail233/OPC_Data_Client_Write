/*
OPC Data Client 範例程式 - 寫入UA server
說明: 一次寫入1個、多個、陣列 點位

請先下載並安裝Data Client 程式開發工具, 在Visual Studio設定好NuGet套件, 便能使用函式庫
執行程式前, 請先重建方案, 套用函式庫

Data Client下載連結
http://www.oneyear.url.tw/index.php/dataclient/menu-dc-download
教學 - 安裝與使用
https://drive.google.com/drive/folders/1BXGkkwC2C9dGUr9k-2GtP7TKYNb1RagY?usp=sharing

程式不能執行? 請別客氣讓我協助您!
-- Kepware OPC專精 壹年資訊 --
侯奕年 Derek
Line ID:oneyear
0932 832 233
www.oneyear.url.tw
derekhou@oneyear.url.tw
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OpcLabs.BaseLib;
using OpcLabs.EasyOpc.UA;
using OpcLabs.EasyOpc.UA.OperationModel;

namespace UAConsole_Write
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpointDescriptor = new UAEndpointDescriptor();
            endpointDescriptor = "opc.tcp://127.0.0.1:49380";  //UA server的URL
            
            /*
            //設定帳號、憑證、加密, 若UA server有設定加密連線, 便需設定
            endpointDescriptor.EndpointSelectionPolicy = new OpcLabs.EasyOpc.UA.Engine.UAEndpointSelectionPolicy(OpcLabs.EasyOpc.UA.Engine.UAMessageSecurityModes.SecuritySignAndEncrypt, "http://opcfoundation.org/UA/SecurityPolicy#Basic128Rsa15");  //指定security policy
            endpointDescriptor.UserIdentity = new OpcLabs.BaseLib.IdentityModel.User.UserIdentity(OpcLabs.BaseLib.IdentityModel.User.UserIdentity.CreateUserNameIdentity("test", "11111111111111"));  //當UA server需要帳號登入時(不允許Anonymous連線時), 需設定UserIdentity, 設定帳號密碼
            */

            EasyUAClient.SharedParameters.EngineParameters.CertificateAcceptancePolicy.AcceptAnyCertificate = true;  //自動信任server的憑證

            var UAClient1 = new EasyUAClient();  //建立UA client實例
            UAAttributeData attributeData;  //存放1個tag資料
            UAAttributeDataResult[] attributeDataResultArray;  //存放多個tag資料, 陣列

            Console.WriteLine("開始寫入tag資料");
            Console.WriteLine("第一次連接需數秒鐘, 請稍等...");

            //一次寫入1個tag
            try
            {
                Console.WriteLine();
                Console.WriteLine("--- 一次寫入1個tag ---");
                Console.WriteLine("寫入12345 到Channel1.Device1.Tag1");
                UAClient1.WriteValue("opc.tcp://127.0.0.1:49380", "ns=2;s=Channel1.Device1.Tag1", 12345);  //寫入tag, 第二個參數"ns=2;s=Channel1.Device1.Tag1" 是tag的位址, 請改成您需要的位址
                Console.WriteLine("寫入陣列[1111,2222,3333]");
                int[] array = {1111,2222,3333};
                UAClient1.WriteValue(endpointDescriptor, "ns=2;s=Channel1.Device1.Array1", array);  //寫入tag, 第二個參數"ns=2;s=Channel1.Device1.Array1" 是tag的位址, 請改成您需要的位址
                Console.WriteLine("完成");
            }
            catch (UAException ex) {
                Console.WriteLine("錯誤: " + ex.ToString());
            }

            //一次寫入多個tag
            try
            {
                Console.WriteLine();
                Console.WriteLine("--- 一次寫入多個tag ---");
                Console.WriteLine("寫入54321 到Channel1.Device1.Tag1");
                Console.WriteLine("寫入888 到Channel1.Device1.Tag2");
                UAClient1.WriteMultipleValues(new[] {
                    new UAWriteValueArguments(endpointDescriptor, "ns=2;s=Channel1.Device1.Tag1", 54321),
                    new UAWriteValueArguments(endpointDescriptor, "ns=2;s=Channel1.Device1.Tag2", 888)
                });  //Tag位址, 請改成您需要的位址
                Console.WriteLine("完成");
            }
            catch (UAException ex) {
                Console.WriteLine("錯誤: " + ex.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("按下Enter停止執行");
            Console.ReadLine();
        }
    
    }
}
