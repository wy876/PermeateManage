   M S B A M L   `   `   `  ����L  HWpfPermeateManage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:管理器	管理器  5   	       管理器.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:管理器     5   	   -   5          � Icon$  Image/men.png��5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � 
ResizeMode$ CanMinimize=�5   	    � Title$ 软件管理器��5   	   $	��600��6   $	��560��6$   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��160 �6   �� 5      .+�5   
   � $��0��6   .+�5      �� -   5        $��Listview��5       � ContextMenu 5      �� 5      .@�5      m� -   5        $	��Add��5   1   $��添加分类��6#   6P   5      5      5      � 5   
   $��1��6   .+�5      �� -   5        $��lstFileManager��5      
������ $��Disabled=�6�   $
��Auto=�6�   $
��True��6�    J� 	AllowDrop$
 True��6S  ��6z   �� ��5      �� 5       .R�5!      � 5"      ��5$      �� 5%      .R�5&      � $��10q�6   $��50��6+   $��60��66    � RowDefinitions 5'      �� 5(   "   $
��Auto �60   6A   �� 5)   "   $
��Auto �60   6A   .+�5+      �� $
��Left=�6;   $��Center=�6V   $��30��6q   $��40��6|   ��6$   �� *Pic+6|   �� 5-      $��1��6>   $��Center=�6K   $
��Left=�6f   $��20��6�   ��6(   �� *Name+6�   5.      5/      51      53      56      57      