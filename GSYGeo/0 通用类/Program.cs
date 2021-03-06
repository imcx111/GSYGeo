﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32;

namespace GSYGeo
{
    /// <summary>
    /// 存储程序通用设置信息的类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 静态字段，存储当前正在操作的项目名称
        /// </summary>
        public static string currentProject = null;

        /// <summary>
        /// 静态字段，设置默认数据存储路径为"我的文档\小熠岩土勘察"
        /// </summary>
        public static string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\小熠岩土勘察";

        /// <summary>
        /// 判断数据存储路径的注册表项是否存在
        /// 返回一个布尔值，true表示注册表项存在，false表示不存在
        /// </summary>
        /// <returns></returns>
        public static bool IsExitProgramPath()
        {
            RegistryKey lcu = Registry.CurrentUser;
            RegistryKey software = lcu.OpenSubKey("SOFTWARE", false);
            RegistryKey program = software.OpenSubKey("GSYGeo", false);
            if (program == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 读取数据存储路径的注册表项
        /// 返回值为注册表中的路径字符串
        /// </summary>
        /// <returns></returns>
        public static string ReadProgramPath()
        {
            RegistryKey lcu = Registry.CurrentUser;
            RegistryKey software = lcu.OpenSubKey("SOFTWARE", false);
            RegistryKey program = software.OpenSubKey("GSYGeo", false);
            if (program != null)
            {
                return program.GetValue("ProgramFilePath").ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 写入数据存储路径的注册表项
        /// 输入值为新的路径字符串，无返回值
        /// </summary>
        /// <param name="_newPath">新的数据存储路径</param>
        public static void SetProgramPath(string _newPath)
        {
            // 读取注册表中的路径键值，如果存在，则写入输入的键值，如果不存在，则新建并写入键值
            RegistryKey lcu = Registry.CurrentUser;
            RegistryKey software = lcu.OpenSubKey("SOFTWARE", false);
            RegistryKey program = software.OpenSubKey("GSYGeo", false);
            if (program != null)
            {
                software = lcu.OpenSubKey("SOFTWARE", true);
                program = software.OpenSubKey("GSYGeo",true);
                program.SetValue("ProgramFilePath", _newPath);
            }
            else
            {
                software = lcu.OpenSubKey("SOFTWARE", true);
                program = software.CreateSubKey("GSYGeo");
                program.SetValue("ProgramFilePath", _newPath);
            }
        }
    }
}
