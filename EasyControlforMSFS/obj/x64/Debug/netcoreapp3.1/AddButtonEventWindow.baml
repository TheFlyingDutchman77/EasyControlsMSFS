   M S B A M L   `   `   `  ����M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  5   	   -    'EasyControlforMSFS.AddButtonEventWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   4xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ,local clr-namespace:EasyControlforMSFS  5   	      MaterialDesignBody#���   5   	     MaterialDesignPaper#���  5	   	   $��Medium"�5
   	   $��14(�5   	     � Title$.  (EasyControls for MSFS - add button event��5   	   $	��400��6:   $	��600��6G    � 
ResizeMode$ NoResize=�5   	    � Icon$ /flight_30822 (1).png��6   ��5   	   :  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��10* �6   .+�5   
   �� -   5        $��Header_button��5      $��1��6+   $��0��6;    �� Orientation$ 
Horizontal=�6H   $��Center=�6a   $��Center=�6~   .+�5      �� $��0,0,0,0q�6   $��Button event definitions��6)   $
��Wrap=�6I   $��Center=�6]   $��Center=�6x   ���	#FBFFFFFF$��18(�6�   $��Normal"�6�   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��3* �6#   �� 5      $��8* �6#    5      �� 5      $
��0.5* �6    �� 5       $��6* �6    �� 5!      $
��1.5* �6    �� 5"      $
��2.5* �6    .+�5%      �� -   5        $��Selectevents_text��5%      $��0��63   $��1��6C   $ Vertical=�6P   $
��Left=�6g   $	��Top=�6�   .+�5&      �� $��Select button nr:��6   $
��Left=�64   $��10,0,0,0q�6O   $��2��6a   $	��Top=�6n   �� 5'      $��3��6   
������ 6'   �� 5(      -   5        $��ButtonEventLabel��5(      $��Select event:��60   $
��Left=�6H   $��10,0,0,0q�6c   $��2��6u   $	��Top=�6�   �� 5)      $��3��6   
������ 6'   �� 5*      -   5        $��ButtonEventOFFlabel��5*      $y�Hidden=�63   $��Select event OFF:��6G   $
��Left=�6c   $��10,0,0,0q�6~   $��2��6�   $	��Top=�6�   �� 5+      $��3��6   
������ 6'   �� 5,      $��Linked to axis (detent):��6   $
��Left=�6;   $��10,0,0,0q�6V   $��2��6h   $	��Top=�6u   �� 5-      $��3��6   
������ 6'   �� 5.      $��Is switch (on/off):��6   $
��Left=�66   $��10,0,0,0q�6Q   $��2��6c   $	��Top=�6p   5/      �� 50      -   5        $��Selectevents_dropdowns��50      $��1��68   $��1��6H   $ Vertical=�6U   $
��Left=�6l   $	��Top=�6�   .+�51      �� $ 
Horizontal=�6   .+�52      �� -   5        $��ButtonNrComboBox��52      $
��Left=�67   $	��Top=�6R   $	��100��6j   6v   �� 53      $"��(press button on controller)��6   $
��Left=�6C   $��10,0,0,0q�6^   $	��Top=�6p   54      �� 55      $��3��6   
������ 6#   �� 56      -   5        $��ButtonEventsComboBox��56      $
��Left=�68   $��2��6S   $	��Top=�6`   $	��300��6x   �� 57      $��3��6   
������ 6'   �� 58      -   5        $��ButtonEventsOFFComboBox��58      $y�Hidden=�6;   $
��Left=�6O   $��2��6j   $	��Top=�6w   $	��300��6�   �� 59      $��3��6   
������ 6'   �� 5:      -	   5        $��ButtonAxisLinkComboBox��5:      $
��Left=�6:   $��2��6U   $	��Top=�6b   $	��300��6z   �� 5;      $��3��6   
������ 6'   �� 5<      -
   5        $��ButtonIsSwitchCheckbox��5<       � ToolTip$� �If this is true, Simconnect event will only be fired once when switch is set to on. Used to prevent continuously firing of events with permanent on/off switches instead of push buttons.��6�   6Q  5=      �� 5>      -   5        $��Newevent_text��5>      $��0��6/   $��2��6?   $ Vertical=�6L   $
��Left=�6c   $	��Top=�6~   .+�5?      �� $��Enter new event:��6   $
��Left=�63   $��10,0,0,0q�6N   $��2��6`   $	��Top=�6m   �� 5@      $��3��6   
������ 6'   5A      �� 5B      -   5        $��New_items_input��5B      $��1��61   $��2��6A   $ Vertical=�6N   $
��Left=�6e   $	��Top=�6�   .+�5C      �� $ 
Horizontal=�6   .+�5D      �� -   5        $��NewEventName��5D      $�� ��63    �� TextWrapping$
 Wrap=�6;   $	��300��6O   
�������
���   �6�   5E      �� 5F      $��3��6   
������ 6'   5G      �� 5H      -   5        $��Auto_select_button��5H      $��0��64   $��3��6D   $ Vertical=�6Q   $
��Left=�6h   $��Center=�6�   .+�5I      �� $��Auto select button:��6   $
��Left=�66   $��10,0,0,0q�6Q   $��2��6c   $��Center=�6p   5J      �� 5K      $��1��6   $��3��6)   $ 
Horizontal=�66   $
��Left=�6O   $��Center=�6j   .+�5L      �� -   5        $��AutoSelectButtonCheckbox��5L      $G AIf this is true, a pressed button will automatically be selected.��6;   6�   �� 5M      -   5        $��SendEventButton��5M      $��12,0,0,0q�60   $	��150��6C   $��14(�6O   .��6{   Send event to sim6�   �� 5N      $��40��6   
������ 6'   �� 5O      -   5        $��80��5O      .��6=   Save6C   �� 5P      $��20��6   
������ 6'   �� 5Q      -   5        $��80��5Q      $��
0,0,20,000q�6=   .��6Q   Exit6W   5R      5S      5T      5U      