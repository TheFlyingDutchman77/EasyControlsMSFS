   M S B A M L   `   `   `  ����M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  5   	   /    )EasyControlforMSFS.Add2ndAxisEventsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   4xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ,local clr-namespace:EasyControlforMSFS  5   	      MaterialDesignBody#���   5   	     MaterialDesignPaper#���  5	   	   $��Medium"�5
   	   $��14(�5   	     � Title$.  (EasyControls for MSFS - add button event��5   	   $	��300��6:   $	��600��6G    � 
ResizeMode$ NoResize=�5   	    � Icon$ /flight_30822 (1).png��6   ��5   	   :  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .��5      �  � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      $	��10* �6   .+�5   
   �� -   5        $��Header_button��5      $��1��6+   $��0��6;    �� Orientation$ 
Horizontal=�6H   $��Center=�6a   $��Center=�6~   .+�5      �� $��0,0,0,0q�6   $��Add 2nd event for axis��6)   $
��Wrap=�6G   $��Center=�6[   $��Center=�6v   ���	#FBFFFFFF$��18(�6�   $��Normal"�6�   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��8* �6#    5      �� 5      $
��0.5* �6    �� 5       $��4* �6    �� 5!      $
��1.5* �6    �� 5"      $��2* �6    .+�5$      �� -   5        $��Selectevents_text��5$      $��0��63   $��1��6C   $ Vertical=�6P   $
��Left=�6g   $	��Top=�6�   .+�5%      �� $��Select axis:��6   $
��Left=�6/   $��10,0,0,0q�6J   $��2��6\   $	��Top=�6i   �� 5&      $��3��6   
������ 6'   �� 5'      $��Select event:��6   $
��Left=�60   $��10,0,0,0q�6K   $��2��6]   $	��Top=�6j   �� 5(      $��3��6   
������ 6'   5)      �� 5*      $��1��6   $��1��6)   .+�5+      �� $ 
Horizontal=�6   $��1��66   $��1��6F   .+�5,      �� -   5        $��SelectAxisComboBox��5,      $
��Left=�6:   $��2��6U   $	��Top=�6b   $	��150��6z   6�   �� 5-      -   5        $��Inverted��5-   <   $��	Inverted?��6   $��70��60   $��Center=�6M   $��	110,0,0,0q�6j   $��2��6}   $	��Top=�6�   �� 5.      -   5        $��Minmax��5.   C   $��Min/max values:��6   $	��110��66   $��Center=�6R   $��10,0,0,0q�6o   $��2��6�   $	��Top=�6�   5/      �� 50      $ 
Horizontal=�6   $��1��66   $��1��6F   $
��Left=�6S   $	��Top=�6n   .+�51      �� -   5        $��AxisEventsComboBox��51      $
��Left=�6:   $��2��6U   $	��Top=�6b   $	��250��6z   �� 52      $��30��6    
������ 6+   �� 53      -   5        $��InvertedAxisCheckBox��53      $��Center=�6;   6Z   �� 54      $��30��6    
������ 6+   �� 55      -   5        $��MinAxisTextBox��55       � ToolTip$� ~Most axis have 0 as min. Some -16383. For throttles, setting a negative nr (e.g. -5000) will allow reverse thrust. See the SDK��64    �� TextAlignment$ Right=�6�   $��50��6�   
���   �$��0��6�   
�������6�   �� 56      $��20��6    
������ 6+   �� 57      -	   5        $��MaxAxisTextBox��57      $ Right=�64   $��50��6K   
���   �$��16383��6i   
�������6w   58      59      �� 5:      -
   5        $��Newevent_text��5:      $��0��6/   $��2��6?   $ Vertical=�6L   $
��Left=�6c   $	��Top=�6~   .+�5;      �� $��Enter new event:��6   $
��Left=�63   $��10,0,0,0q�6N   $��2��6`   $	��Top=�6m   �� 5<      $��3��6   
������ 6'   5=      �� 5>      -   5        $��New_items_input��5>      $��1��61   $��2��6A   $ Vertical=�6N   $
��Left=�6e   $	��Top=�6�   .+�5?      �� -   5        $��NewEventName��5?      $�� ��6/    �� TextWrapping$
 Wrap=�67   $	��250��6K   
�������
���   �6}   �� 5@      $��3��6   
������ 6'   5A      �� 5B      $��1��6   $��3��6)   $ 
Horizontal=�66   $��Right=�6O   $��Center=�6k   .+�5C      �� -   5        $��80��5C      .��6=   Save6C   �� 5D      $��20��6   
������ 6'   �� 5E      -   5        $��80��5E      $��
0,0,20,000q�6=   .��6Q   Exit6W   5F      5G      5H      5I      