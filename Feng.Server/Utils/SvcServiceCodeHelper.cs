﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Feng.Server.Utils
{
    public class SvcServiceCodeHelper
    {
        public static void Initialize()
        {
            Feng.Utils.IOHelper.HardDirectoryDelete(".\\GeneratedCode");
            Feng.Utils.IOHelper.TryCreateDirectory(".\\GeneratedCode\\");
            //System.IO.File.Delete(".\\GeneratedCode\\web.config.include");
        }
        public static void GenerateDataSearchViewWSFiles(string wintabName)
        {
            Feng.Server.Utils.TypeHelper.GenerateCodeFromWintabInfo(wintabName);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateServiceInterface4Wintab(wintabName);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateServiceClass4Wintab(wintabName);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateSvcFile4Wintab(wintabName);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateConfigFile4Wintab(wintabName);
        }
        public static void GenerateDataSearchWSFiles(Type srcType)
        {
            Feng.Server.Utils.TypeHelper.GenerateCodeFromType(srcType);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateServiceInterface4Type(srcType);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateServiceClass4Type(srcType);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateSvcFile4Type(srcType);
            Feng.Server.Utils.SvcServiceCodeHelper.GenerateConfigFile4Type(srcType);
        }

        public static void GenerateServiceInterface4Wintab(string wintabName)
        {
            string fileName = ".\\GeneratedCode\\" + "DSRSI_" + wintabName + ".cs";
            string interfaceLine = string.Format("{0} : Feng.Server.Service.IDataSearchViewRestService<{1}>",
                    "DSRSI_" + wintabName, "WinTab_" + wintabName);
            GenerateServiceInterface(fileName, interfaceLine);
        }
        public static void GenerateServiceInterface4Type(Type srcType)
        {
            string fileName = fileName = ".\\GeneratedCode\\" + "DSSI_" + srcType.Name + ".cs";
            string interfaceLine = string.Format("{0} : Feng.Server.Service.IDataSearchRestService<{1}>, Feng.Server.Service.IDataOperationRestService<{1}>",
                    "DSSI_" + srcType.Name, "Type_" + srcType.Name);
            GenerateServiceInterface(fileName, interfaceLine);
        }
        private static void GenerateServiceInterface(string fileName, string interfaceLine)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(string.Format("namespace {0}", "Feng.AutoGenerated"));
                sw.WriteLine("{");
                sw.WriteLine("\t[System.ServiceModel.ServiceContract]");
                sw.WriteLine(string.Format("\tpublic interface {0}", interfaceLine));
                sw.WriteLine("\t{ }");
                sw.WriteLine("}");
            }
        }

        public static void GenerateServiceClass4Wintab(string wintabName)
        {
            string fileName = ".\\GeneratedCode\\" + "DSRS_" + wintabName + ".svc.cs";
            StringBuilder classLine = new StringBuilder();
            classLine.AppendLine(string.Format("{0} : Feng.Server.Service.DataSearchViewService<{1}>, {2}",
                    "DSRS_" + wintabName, "WinTab_" + wintabName, "DSRSI_" + wintabName));
            classLine.AppendLine("\t{");
            classLine.AppendLine(string.Format("\t\t{0}() {{ m_winTabName = \"{1}\"; }}", "DSRS_" + wintabName, wintabName));
            classLine.AppendLine("\t}");

            GenerateServiceClass(fileName, classLine.ToString());
        }
        public static void GenerateServiceClass4Type(Type srcType)
        {
            string fileName = ".\\GeneratedCode\\" + "DSS_" + srcType.Name + ".svc.cs";
            StringBuilder classLine = new StringBuilder();
            classLine.AppendLine(string.Format("{0} : Feng.Server.Service.DataOperationService<{1}, {3}>, {2}",
                    "DSS_" + srcType.Name, "Type_" + srcType.Name, "DSSI_" + srcType.Name, srcType.Name));
            classLine.AppendLine("\t{");
            //classLine.AppendLine((string.Format("\t\t{0}() {{ m_winTabName = \"{1}\"; }}", "DSS_" + srcType.Name, srcType.Name));
            classLine.AppendLine("\t}");

            GenerateServiceClass(fileName, classLine.ToString());
        }
        private static void GenerateServiceClass(string fileName, string classLine)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(string.Format("namespace {0}", "Feng.AutoGenerated"));
                sw.WriteLine("{");
                sw.WriteLine(string.Format("\tpublic class {0}", classLine));
                sw.WriteLine("}");
            }
        }

        public static void GenerateSvcFile4Wintab(string wintabName)
        {
            string fileName = ".\\GeneratedCode\\" + "DSRS_" + wintabName + ".svc";
            string serviceName = "DSRS_" + wintabName;
            GenerateSvcFile(fileName, serviceName);
        }
        public static void GenerateSvcFile4Type(Type srcType)
        {
            string fileName = ".\\GeneratedCode\\" + "DSS_" + srcType.Name + ".svc";
            string serviceName = "DSS_" + srcType.Name;
            GenerateSvcFile(fileName, serviceName);
        }
        private static void GenerateSvcFile(string fileName, string srviceName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(string.Format("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{0}\" CodeBehind=\"{1}\" %>",
                    "Feng.AutoGenerated." + srviceName,
                    "Feng.AutoGenerated." + srviceName + ".svc.cs"));
            }
        }
        private static void GenerateConfigFile4Wintab(string wintabName)
        {
            string serviceName = string.Format("Feng.AutoGenerated.DSRS_{0}", wintabName);
            string interfaceName = string.Format("Feng.AutoGenerated.DSRSI_{0}", wintabName);
            GenerateConfigFile(serviceName, interfaceName);
        }
        public static void GenerateConfigFile4Type(Type srcType)
        {
            string serviceName = string.Format("Feng.AutoGenerated.DSS_{0}", srcType.Name);
            string interfaceName = string.Format("Feng.AutoGenerated.DSSI_{0}", srcType.Name);
            GenerateConfigFile(serviceName, interfaceName);
        }

        private static void GenerateConfigFile(string serviceName, string interfaceName)
        {
            string fileName = ".\\GeneratedCode\\web.config.include";
               
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(string.Format("<service behaviorConfiguration=\"DefaultServiceBehavior\" name=\"{0}\">", serviceName));
                sw.WriteLine(string.Format("<endpoint address=\"\" behaviorConfiguration=\"web\" binding=\"webHttpBinding\" bindingConfiguration=\"LargeString\" contract=\"{0}\"/>", interfaceName));
                sw.WriteLine("<endpoint address=\"mex\" binding=\"mexHttpBinding\" contract=\"IMetadataExchange\"/>");
                sw.WriteLine("</service>");
            }
        }
    }
}
