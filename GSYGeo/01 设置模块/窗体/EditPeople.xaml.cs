﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GSYGeo
{
    /// <summary>
    /// EditPeople.xaml 的交互逻辑
    /// </summary>
    public partial class EditPeople : Window
    {
        /// <summary>
        /// 判断是新建还是编辑
        /// </summary>
        private bool isNew;

        /// <summary>
        /// 定义旧人员姓名变量
        /// </summary>
        string oldName = null;

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public EditPeople()
        {
            InitializeComponent();

            isNew = true;
            this.PeopleNameTextBox.Focus();
        }

        /// <summary>
        /// 带一个参数的构造函数，参数为人员姓名
        /// </summary>
        /// <param name="_name">人员姓名</param>
        public EditPeople(string _name)
        {
            InitializeComponent();

            oldName = _name;
            PeopleNameTextBox.Text = _name;
            isNew = false;
            this.PeopleNameTextBox.Focus();
        }

        /// <summary>
        /// 点击"确定"，新建和编辑人员时执行不同的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PeopleNameTextBox.Text) || string.IsNullOrWhiteSpace(PeopleNameTextBox.Text))
            {
                MessageBox.Show("人员姓名不能为空！", "姓名输入错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.PeopleNameTextBox.Focus();
                return;
            }
            if (isNew == true)
            {
                if (SettingDataBase.IsExistPeople(PeopleNameTextBox.Text))
                {
                    MessageBox.Show("已有同名人员，请输入不同的名字。", "重名", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                if (SettingDataBase.IsExistPeople(oldName))
                {
                    this.DialogResult = true;
                }
            }
        }
    }
}
