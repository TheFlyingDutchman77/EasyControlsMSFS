   M S B A M L   `   `   `  ���� M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  #    EasyControlforMSFS.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  ,local clr-namespace:EasyControlforMSFS  -      MaterialDesignBody#���     MaterialDesignPaper#���  $��Medium"�$��14(�  � 
ResizeMode$  NoResize=� � Title$ EasyControls for MSFS��$	��550��$	��700�� � Icon$ /flight_30822 (1).png����:  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .���  � RowDefinitions �� $��2* ��� $	��10* �.+��� -   $��Header��$��1��$��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��0,0,0,0q�$��EasyControls for MSFS��$
��Wrap=�$��Center=�$��Center=����	#FBFFFFFF$��20(�� $��1�� � ColumnDefinitions �� $��1* ��� $	��500 ��� $��1* � �� $
��0.1* ��� $��2* ��� $
��3.5* ��� $
��0.2* ��� $��1* �.+��� $ Vertical=�$��1��$��1��$��Center=�.+��� -   $��Aircraft_text��$ 
Horizontal=�$
��Left=�$��Center=�.+��� $��Select profile:��$	��125��$
��Left=�$��0,0,0,0q�$��2��$	��Top=��� -   $��SelectAircraftComboBox��$��10,0,0,0q�$
��Left=�$��2��$	��Top=�$	��200���� $��20��
������ �� -   $��Aircraft_title��$ 
Horizontal=�$
��Left=�$��Center=�.+��� $��MSFS aircraft title:��$��Center=�$	��125��$
��Left=�$��0,0,0,0q��� -   $��TitleTextBlock��$��no title received��$��Center=�$	��350��$��10,0,0,0q�
��������� $��10��
������ �� -   $��MapTitleButton��$
��Left=�$��0,0,0,0q�$	��Top=�$y�Hidden=�.��Map title to selected profile�� -   $��Aircrafty_input��$��1��$��1��$ 
Horizontal=�$
��Left=�$��Center=��� -	   $��Messages��$��1��$��2��$ Vertical=�$
��Left=�$��Center=�.+��� $��System messages:���� -
    ��Name$ MessageTextBox��" �� VerticalScrollBarVisibility$ Visible=�$��0,0,0,0q�$	��500��$	��125�� �� TextWrapping$
 Wrap=�
���iii�
�������$��Light"��� $��20��
������ �� -   	 ��Name$	 
Simconnect��$ 
Horizontal=�.+��� $��Simconnect status:��$	��125��$
��Left=�$��	0,0, 10,0q�$��2��$	��Top=�A� -   
 A�Name$
 SimconnectStatusEllipse��
���  ��$��15��$��15��$��0���� -   $ Test1TextBox��$	��100��$��10,0,0,0q�$y�Hidden=��� -   $ Test2TextBox��$	��100��$��10,0,0,0q�$y�Hidden=��� -   $��Buttonsexit��$��1��$��4��$ 
Horizontal=�$��Right=�$��Center=�.+��� -   $��FSUIPCReload��$
��Left=�$��0,0,0,0q�$	��Top=�.��Reload FSUIPC�� $��20��
������ �� -   $��Define events��$��0,0,0,20q��� $��20��
������ �� -   $
��Left=�$��0,0,0,0q�$	��Top=�.��Exit