   M S B A M L   `   `   `  ���� M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  /    )EasyControlforMSFS.Add2ndAxisEventsWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  4xctk)http://schemas.xceed.com/wpf/xaml/toolkit  ,local clr-namespace:EasyControlforMSFS     MaterialDesignBody#���     MaterialDesignPaper#���  $��Medium"�$��14(�  � Title$.  (EasyControls for MSFS - add button event��$	��300��$	��600�� � 
ResizeMode$ NoResize=� � Icon$ /flight_30822 (1).png����:  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .���  � RowDefinitions �� $��3* ��� $	��10* �.+��� -   $��Header_button��$��1��$��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��0,0,0,0q�$��Add 2nd event for axis��$
��Wrap=�$��Center=�$��Center=����	#FBFFFFFF$��18(�$��Normal"�� $��1�� � ColumnDefinitions �� $��2* ��� $��8* � �� $
��0.5* ��� $��4* ��� $
��1.5* ��� $��2* �.+��� -   $��Selectevents_text��$��0��$��1��$ Vertical=�$
��Left=�$	��Top=�.+��� $��Select axis:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Select event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��1��$��1��.+��� $ 
Horizontal=�$��1��$��1��.+��� -   $��SelectAxisComboBox��$
��Left=�$��2��$	��Top=�$	��150���� -   $��Inverted��$��	Inverted?��$��70��$��Center=�$��	110,0,0,0q�$��2��$	��Top=��� -   $��Minmax��$��Min/max values:��$	��110��$��Center=�$��10,0,0,0q�$��2��$	��Top=��� $ 
Horizontal=�$��1��$��1��$
��Left=�$	��Top=�.+��� -   $��AxisEventsComboBox��$
��Left=�$��2��$	��Top=�$	��250���� $��30��
������ �� -   $��InvertedAxisCheckBox��$��Center=��� $��30��
������ �� -   $��MinAxisTextBox�� � ToolTip$� ~Most axis have 0 as min. Some -16383. For throttles, setting a negative nr (e.g. -5000) will allow reverse thrust. See the SDK�� �� TextAlignment$ Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -	   $��MaxAxisTextBox��$ Right=�$��50��
���   �$��16383��
��������� -
   $��Newevent_text��$��0��$��2��$ Vertical=�$
��Left=�$	��Top=�.+��� $��Enter new event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -   $��New_items_input��$��1��$��2��$ Vertical=�$
��Left=�$	��Top=�.+��� -   $��NewEventName��$�� �� �� TextWrapping$
 Wrap=�$	��250��
�������
���   ��� $��3��
������ �� $��1��$��3��$ 
Horizontal=�$��Right=�$��Center=�.+��� -   $��80��.��Save�� $��20��
������ �� -   $��80��$��
0,0,20,000q�.��Exit