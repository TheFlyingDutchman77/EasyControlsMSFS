   M S B A M L   `   `   `  ����M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  5   	   #    EasyControlforMSFS.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   ,local clr-namespace:EasyControlforMSFS  5   	   -   5           MaterialDesignBody#���   5   	     MaterialDesignPaper#���  5   	   $��Medium"�5	   	   $��14(�5
   	     � 
ResizeMode$  NoResize=�5   	    � Title$ EasyControls for MSFS��5   	   $	��550��6'   $	��700��64    � Icon$ /flight_30822 (1).png��6@   ��5   	   :  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��10* �6   .+�5   
   �� -   5        $��Header��5      $��1��6$   $��0��64    �� Orientation$ 
Horizontal=�6A   $��Center=�6Z   $��Center=�6w   .+�5      �� $��0,0,0,0q�6   $��EasyControls for MSFS��6)   $
��Wrap=�6F   $��Center=�6Z   $��Center=�6u   ���	#FBFFFFFF$��20(�6�   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $	��500 �6#   �� 5      $��1* �6#    5      �� 5       $
��0.1* �6    �� 5!      $��2* �6    �� 5"      $
��3.5* �6    �� 5#      $
��0.2* �6    �� 5$      $��1* �6    .+�5&      �� $ Vertical=�6   $��1��60   $��1��6@   $��Center=�6M   .+�5'      �� -   5        $��Aircraft_text��5'      $ 
Horizontal=�63   $
��Left=�6L   $��Center=�6g   .+�5(      �� $��Select profile:��6   $	��125��66   $
��Left=�6C   $��0,0,0,0q�6^   $��2��6o   $	��Top=�6|   �� 5)      -   5        $��SelectAircraftComboBox��5)      $��10,0,0,0q�6=   $
��Left=�6O   $��2��6j   $	��Top=�6w   $	��200��6�   6�   5*      �� 5+      $��20��6   
������ 6(   �� 5,      -   5        $��Aircraft_title��5,      $ 
Horizontal=�64   $
��Left=�6M   $��Center=�6h   .+�5-      �� $��MSFS aircraft title:��6   $��Center=�6;   $	��125��6V   $
��Left=�6c   $��0,0,0,0q�6~   �� 5.      -   5        $��TitleTextBlock��5.       $��no title received��66   $��Center=�6P   $	��350��6l   $��10,0,0,0q�6x   
�������6�   5/      �� 50      $��10��6   
������ 6(   �� 51      -   5        $��MapTitleButton��51      $
��Left=�6/   $��0,0,0,0q�6J   $	��Top=�6[   $y�Hidden=�6�   .��6�   Map title to selected profile6�   52      �� 53      -   5        $��Aircrafty_input��53      $��1��61   $��1��6A   $ 
Horizontal=�6N   $
��Left=�6g   $��Center=�6�   54      �� 55      -	   5        $��Messages��55      $��1��6*   $��2��6:   $ Vertical=�6G   $
��Left=�6^   $��Center=�6y   .+�56      �� $��System messages:��6   �� 57      -
   5         ��Name$ MessageTextBox��57      " �� VerticalScrollBarVisibility$ Visible=�63   $��0,0,0,0q�6Y   $	��500��6k   $	��125��6w    �� TextWrapping$
 Wrap=�6�   
���iii�
�������$��Light"�6�   �� 58      $��20��6   
������ 6(   �� 59      -   5        	 ��Name$	 
Simconnect��59      $ 
Horizontal=�62   .+�5:      �� $��Simconnect status:��6   $	��125��69   $
��Left=�6F   $��	0,0, 10,0q�6a   $��2��6t   $	��Top=�6�   A� 5;      -   5        
 A�Name$
 SimconnectStatusEllipse��5;      
���  ��$��15��6K   $��15��6W   $��0��6b   �� 5<      -   5        $ Test1TextBox��5<      $	��100��64   $��10,0,0,0q�6@   $y�Hidden=�6R   �� 5=      -   5        $ Test2TextBox��5=      $	��100��64   $��10,0,0,0q�6@   $y�Hidden=�6R   5>      5?      �� 5@      -   5        $��Buttonsexit��5@      $��1��6-   $��4��6=   $ 
Horizontal=�6J   $��Right=�6c   $��Center=�6   .+�5A      �� -   5        $��
MidiReload��5A      $
��Left=�6+   $��0,0,0,0q�6F   $	��Top=�6W   .��6�   Reload MIDI6�   �� 5B      $��20��6   
������ 6'   �� 5C      -   5        $��FSUIPCReload��5C      $
��Left=�6-   $��0,0,0,0q�6H   $	��Top=�6Y   .��6�   Reload FSUIPC6�   �� 5D      $��20��6   
������ 6'   �� 5E      -   5        $��Define events��5E      $��0,0,0,20q�61   6C   �� 5F      $��20��6   
������ 6'   �� 5G      -   5        $
��Left=�5G      $��0,0,0,0q�64   $	��Top=�6E   .��6v   Exit6|   5H      5J      5M      5N      