   M S B A M L   `   `   `  ���� M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  -    'EasyControlforMSFS.AddButtonEventWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  4xctk)http://schemas.xceed.com/wpf/xaml/toolkit  ,local clr-namespace:EasyControlforMSFS     MaterialDesignBody#���     MaterialDesignPaper#���  $��Medium"�$��14(�  � Title$.  (EasyControls for MSFS - add button event��$	��400��$	��600�� � 
ResizeMode$ NoResize=� � Icon$ /flight_30822 (1).png����:  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .���  � RowDefinitions �� $��2* ��� $	��10* �.+��� -   $��Header_button��$��1��$��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��0,0,0,0q�$��Button event definitions��$
��Wrap=�$��Center=�$��Center=����	#FBFFFFFF$��18(�$��Normal"�� $��1�� � ColumnDefinitions �� $��3* ��� $��8* � �� $
��0.5* ��� $��6* ��� $
��1.5* ��� $
��2.5* �.+��� -   $��Selectevents_text��$��0��$��1��$ Vertical=�$
��Left=�$	��Top=�.+��� $��Select button nr:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -   $��ButtonEventLabel��$��Select event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -   $��ButtonEventOFFlabel��$y�Hidden=�$��Select event OFF:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Linked to axis (detent):��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Is switch (on/off):��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� -   $��Selectevents_dropdowns��$��1��$��1��$ Vertical=�$
��Left=�$	��Top=�.+��� $ 
Horizontal=�.+��� -   $��ButtonNrComboBox��$
��Left=�$	��Top=�$	��100���� $"��(press button on controller)��$
��Left=�$��10,0,0,0q�$	��Top=��� $��3��
������ �� -   $��ButtonEventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��3��
������ �� -   $��ButtonEventsOFFComboBox��$y�Hidden=�$
��Left=�$��2��$	��Top=�$	��300���� $��3��
������ �� -	   $��ButtonAxisLinkComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��3��
������ �� -
   $��ButtonIsSwitchCheckbox�� � ToolTip$� �If this is true, Simconnect event will only be fired once when switch is set to on. Used to prevent continuously firing of events with permanent on/off switches instead of push buttons.���� -   $��Newevent_text��$��0��$��2��$ Vertical=�$
��Left=�$	��Top=�.+��� $��Enter new event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -   $��New_items_input��$��1��$��2��$ Vertical=�$
��Left=�$	��Top=�.+��� $ 
Horizontal=�.+��� -   $��NewEventName��$�� �� �� TextWrapping$
 Wrap=�$	��300��
�������
���   ��� $��3��
������ �� -   $��Auto_select_button��$��0��$��3��$ Vertical=�$
��Left=�$��Center=�.+��� $��Auto select button:��$
��Left=�$��10,0,0,0q�$��2��$��Center=��� $��1��$��3��$ 
Horizontal=�$
��Left=�$��Center=�.+��� -   $��AutoSelectButtonCheckbox��$G AIf this is true, a pressed button will automatically be selected.���� -   $��SendEventButton��$��12,0,0,0q�$	��150��$��14(�.��Send event to sim�� $��40��
������ �� -   $��80��.��Save�� $��20��
������ �� -   $��80��$��
0,0,20,000q�.��Exit