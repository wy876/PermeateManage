using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 管理器
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<LVData> LVDatas = new ObservableCollection<LVData>();

        ObservableCollection<PData> PData = new ObservableCollection<PData>();
        Ico ico = new Ico();
        public MainWindow()
        {
            InitializeComponent();
            
            lstFileManager.ItemsSource = LVDatas;
            

        }
        
        public string FileIco = string.Empty; //存放ico 路径
        string FileOut = string.Empty;  //存放 软件文件路径
        public string filename = string.Empty; //获取文件名 和文件夹名称
       
        List<string> listS = new List<string>();  //定义数组 把defaults 数据 添加到数组中
        /// <summary>
        /// 初始化 函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
            string path3 = Directory.GetCurrentDirectory(); //获取当前目录
            string PF = path3 + "/path.txt";

            // Check if the path.txt file exists
            if (!System.IO.File.Exists(PF))
            {
                // If the file doesn't exist, create it
                using (FileStream fs = System.IO.File.Create(PF))
                {
                    // You can add some content to the file if needed
                    // For example, you can write a default line to the file
                    byte[] defaultContent = new System.Text.UTF8Encoding(true).GetBytes("");
                    fs.Write(defaultContent, 0, defaultContent.Length);
                }

                Console.WriteLine("File created: " + PF);
            }
            else
            {
                Console.WriteLine("File already exists: " + PF);
            }

             //读取path.txt文件内容
            string[] lines = System.IO.File.ReadAllLines(PF, Encoding.Default);

            //从lines遍历内容
            foreach (string line in lines)
            {
                string pattern = @"(\\[^\\/'\""])"; //正则
                string s = Regex.Replace(line, pattern, "\\$1"); //过滤特殊字符
                PData rt = JsonConvert.DeserializeObject<PData>(s); //解析json 数据
                if (Directory.Exists(FileOut)) //判断是否 文件夹
                {
                    string[] files = Directory.GetDirectories(FileOut);
                    foreach (string Element in files)
                    {
                        filename = Element;
                    }
                }
                else
                {
                    filename = Path.GetFileName(rt.FilePath); //获取文件名 带后缀
                }
               

                //添加ID 到数组
                if (!listS.Contains(rt.ID))
                    listS.Add(rt.ID);
               
                LVDatas.Add(new LVData { Name = filename, Pic = new BitmapImage(new Uri(rt.IcoPath)) });
                
               


            }
            //List用于存储从数组里取出来的不相同的元素
            List<string> listString = new List<string>();

            foreach (string eachString in listS)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }

            //最后从List里取出各个字符串进行操作
            foreach (string eachString in listString)
            {

                Listview.Items.Add(eachString);
            }
            
                



        }
        

        /// <summary>
        /// Returns an icon representation of an image that is contained in the specified file.
        /// </summary>
        /// <param name="executablePath"></param>
        /// <returns></returns>
        public static Icon ExtractIconFromFilePath(string executablePath)
        {
            Icon result = (Icon)null;

            try
            {
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                result = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to extract the icon from the binary");
            }

            return result;
        }
        /// <summary>
        ///  拖入事件(拖入时执行)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFileManager_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

        }

        /// <summary>
        /// 拖入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            object a;
            a = Listview.SelectedItem;
            
            if(a != null)
            {
                string File_Path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString(); //获取文件路径

                
                if (File_Path.ToLower().EndsWith("lnk"))  //判断 lnk快捷方式
                {
                    WshShell shell = new WshShell();
                    IWshShortcut wshShortcut = (IWshShortcut)shell.CreateShortcut(File_Path);
                    FileOut = wshShortcut.TargetPath;  //文件目录
                }
                else
                {
                    FileOut = File_Path;

                }

                if (Directory.Exists(File_Path)) //判断是否 文件夹
                {
                   
                    string[] files = Directory.GetDirectories(File_Path);
                    foreach (string Element in files)
                    {
                        filename = Element;
                    }

                    string path3 = Directory.GetCurrentDirectory(); //获取当前目录
                    FileIco = path3 + "/Ico/folder-closed.png";

                }
                else  //文件
                {
                    filename = Path.GetFileName(FileOut); //获取文件名 带后缀

                    string path3 = Directory.GetCurrentDirectory(); //获取当前目录
                    string[] F = filename.Split(".");
                    FileIco = path3 + "/Ico/" + F[0] + ".ico";


                    Icon theIcon = ExtractIconFromFilePath(FileOut); //获取ico图标
                    theIcon.ToBitmap().Save(FileIco);  //保存图标
                }




                //保存文件 文件路径 和 ico路径
                try
                {

                    string Fopen = String.Format("{{\"ID\":\"{0}\",\"FilePath\":\"{1}\",\"IcoPath\":\"{2}\",}}", a.ToString(), FileOut, FileIco);
                    System.IO.File.AppendAllText(@"path.txt", Fopen + Environment.NewLine);

                }
                catch (Exception ex)
                {
                }

                LVDatas.Add(new LVData { Name = filename, Pic = new BitmapImage(new Uri(FileIco)) }); //listbox 添加数据
                
            }
            

        }
        /// <summary>
        /// 启动软件
        /// </summary>
        /// <param name="filepath"></param>
        public void RunSysExe(string filepath)
        {
            try
            {
               
                if (Directory.Exists(filepath)) //判断是否 文件夹
                {
                  
                    Process.Start("explorer.exe", filepath);
                }
                else
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = filepath;
                    processStartInfo.CreateNoWindow = false;
                    processStartInfo.UseShellExecute = true;
                    Process.Start(processStartInfo);
                }
                
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add menu = new Add();
            menu.sendMok = Recevie;
            menu.Show();
        }
        /// <summary>
        /// 接受子窗口的数据
        /// </summary>
        /// <param name="value"></param>
        public void Recevie(string value)
        {
           
            Listview.Items.Add(value);

        }

        /// <summary>
        /// listview 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listview_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            object a;
            a = Listview.SelectedItem;
            if(a != null)
            {
                LVDatas.Clear();
                string path3 = Directory.GetCurrentDirectory(); //获取当前目录
                string PF = path3 + "/path.txt";
                string[] lines = System.IO.File.ReadAllLines(PF, Encoding.Default);


                foreach (string line in lines)
                {
                    string pattern = @"(\\[^\\/'\""])"; //正则
                    string s = Regex.Replace(line, pattern, "\\$1"); //过滤特殊字符
                    PData rt = JsonConvert.DeserializeObject<PData>(s); //解析json 数据

                    
                    filename = Path.GetFileName(rt.FilePath); //获取文件名 带后缀
                    

                    //添加ID 到数组
                    if (!listS.Contains(rt.ID))
                        listS.Add(rt.ID);

                    if (rt.ID == a.ToString())
                    {
                        LVDatas.Add(new LVData { Name = filename, Pic = new BitmapImage(new Uri(rt.IcoPath)) });
                    }


                }


            }
        }

        /// <summary>
        /// 获取 listbox 选中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFileManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filePath = string.Empty;
            foreach (object o in lstFileManager.SelectedItems)
            {
                string OutExe = (o as LVData).Name;

                string path3 = Directory.GetCurrentDirectory(); //获取当前目录
                string PF = path3 + "/path.txt";
                string[] lines = System.IO.File.ReadAllLines(PF, Encoding.Default);


                foreach (string line in lines)
                {
                    string pattern = @"(\\[^\\/'\""])"; //正则
                    string s = Regex.Replace(line, pattern, "\\$1"); //过滤特殊字符
                    PData rt = JsonConvert.DeserializeObject<PData>(s); //解析json 数据

                    filePath = Path.GetFileName(rt.FilePath); //获取文件名 带后缀
                    
                    //string filename = Path.GetFileName(rt.FilePath); //获取文件名 带后缀
                    if(OutExe == filePath)
                    {
                     
                        RunSysExe(rt.FilePath);
                    }


                }
            }


        }

    }
}
