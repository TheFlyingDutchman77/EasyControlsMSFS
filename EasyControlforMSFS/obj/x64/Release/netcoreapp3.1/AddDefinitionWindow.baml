   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  M IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS ,   &EasyControlforMSFS.AddDefinitionWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      @d2http://schemas.microsoft.com/expression/blend/2008      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  ,local clr-namespace:EasyControlforMSFS     MaterialDesignBody#���     MaterialDesignPaper#���  $��Medium"�$��14(�  � 
ResizeMode$  NoResize=� � Title$4 .EasyControls for MSFS - add control definition��$	��500��$	��800�� � Icon$ /flight_30822 (1).png����:  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .���  � RowDefinitions �� $��1* ��� $	��10* �.+��� -   $��Header��$��1��$��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��0,0,0,0q�$ ��Aircraft event definitions��$
��Wrap=�$��Center=�$��Center=����	#FBFFFFFF$��20(�� $��1�� � ColumnDefinitions �� $��3* ��� $��8* � �� $��20 ��� $��40 ��� $��4* ��� $��60 ��� $��90 �.+��� -   $��Controller_text��$��0��$��1��$ Vertical=�$
��Left=�$��Center=�.+��� $��Select controller:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� -   $��Controller_input��$��1��$��1��$ 
Horizontal=�$
��Left=�$��Center=�.+��� -   $��ControllersComboBox��$
��Left=�$��2��$	��Top=�$	��300�� �� 
IsEditable$
 True���� $��10��
������ �� -   $��ControllerConnectedLabel��$��Check if connected��$��0,0,0,0q�$��1��$��Center=�$y�Hidden=��� -   $��Controller_connected��$��2��$��1��$ Vertical=�$
��Left=�$��Center=��� $ Vertical=�$��0��$��2��.+��� -   $��ACname_text��$ Vertical=�$
��Left=�$��Center=�.+��� $��Select profile:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Axis 1 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Axis 2 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Axis 3 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� -   $��Axis45labels��$y�Hidden=�$ Vertical=�.+��� $��3��
������ �� -	   $��
Axis4label��$��Axis 4 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -
   $��
Axis5label��$��Axis 5 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� -   $��
Axis6label��$��Axis 6 event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� -   $��ACname_input��$��1��$��2��$ Vertical=�$
��Left=�$	��Top=�.+��� $ Vertical=�$��Center=�.+��� $ 
Horizontal=�.+��� -   $��ACnamesComboBox��$
��Left=�$��2��$	��Top=�$	��300�� �� 
IsReadOnly$
 True���� -   $��Inverted��$��	Inverted?��$��70��$��Center=�$��10,0,0,0q�$��2��$	��Top=��� -   $��Minmax��$��Min/max values:��$	��110��$��Center=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $ 
Horizontal=�.+��� -   $��Axis1EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -   $��InvertedAxis1CheckBox��$��Center=��� $��30��
������ �� -   $��MinAxis1TextBox�� � ToolTip$� ~Most axis have 0 as min. Some -16383. For throttles, setting a negative nr (e.g. -5000) will allow reverse thrust. See the SDK��	 �� TextAlignment$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -   $��MaxAxis1TextBox��$	 Right=�$��50��
���   �$��16383��
��������� $��3��
������ �� $ 
Horizontal=�.+��� -   $��Axis2EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -   $��InvertedAxis2CheckBox��$��Center=��� $��30��
������ �� -   $��MinAxis2TextBox��$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -   $��MaxAxis2TextBox��$	 Right=�$��50��
���   �$��16383��
��������� $��3��
������ �� $ 
Horizontal=�.+��� -   $��Axis3EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -   $��InvertedAxis3CheckBox��$��Center=��� $��30��
������ �� -   $��MinAxis3TextBox��$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -   $��MaxAxis3TextBox��$	 Right=�$��50��
���   �$��16383��
��������� $��3��
������ �� -   $��StackPanel46Axis��$ Vertical=�$y�Hidden=�.+��� -   $��StackPanelAxis4��$ 
Horizontal=�.+��� -   $��Axis4EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -   $��InvertedAxis4CheckBox��$��Center=��� $��30��
������ �� -    $��MinAxis4TextBox��$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -!   $��MaxAxis4TextBox��$	 Right=�$��50��
���   �$��16383��
��������� $��3��
������ �� -"   $��StackPanelAxis5��$ 
Horizontal=�.+��� -#   $��Axis5EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -$   $��InvertedAxis5CheckBox��$��Center=��� $��30��
������ �� -%   $��MinAxis5TextBox��$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -&   $��MaxAxis5TextBox��$	 Right=�$��50��
���   �$��16383��
��������� $��3��
������ �� -'   $��StackPanelAxis6��$ 
Horizontal=�.+��� -(   $��Axis6EventsComboBox��$
��Left=�$��2��$	��Top=�$	��300���� $��30��
������ �� -)   $��InvertedAxis6CheckBox��$��Center=��� $��30��
������ �� -*   $��MinAxis6TextBox��$	 Right=�$��50��
���   �$��0��
��������� $��20��
������ �� -+   $��MaxAxis6TextBox��$	 Right=�$��50��
���   �$��16383��
��������� -,   $��Add_button_event��$��3��$��0��$ 
Horizontal=�$
��Left=�.+��� --   $��Add_button_event_button��$��10,0,0,0q�.��Add button events�� -.   $��Add_2ndaxisevent_event��$��3��$��1��$ 
Horizontal=�$
��Left=�.+��� -/   $��Add_2nd_axis_event_button��$	��175��$��0,0,0,0q�.��Add 2nd event for axis�� $��10��
������ �� -0   $��MIDIMapperWindow��$	��115��$��0,0,30,0q�.��MIDI mapper�� -1   $��AddAxis46CheckBox��$��Center=��� $��Add axis 4-6��$
��Left=�$��10,0,0,0q�$��Center=��� -2   $��	New_items��$��0��$��7��$ Vertical=�$
��Left=�$��Center=�.+��� $��Enter new profile:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $��3��
������ �� $��Enter new event:��$
��Left=�$��10,0,0,0q�$��2��$	��Top=��� $ 
Horizontal=�$��1��$��7��.+��� -3   $��New_items_input��$��1��$��7��$ Vertical=�$
��Left=�$��Center=�.+��� -4   $��NewAircraftName��$Z TType here a new aircraft name and hit enter. Aircraft will then be selectable above.��$�� ��
 �� TextWrapping$

 Wrap=�$	��300��
�������
���   ��� $��3��
������ �� -5   $��NewEventName��$�� ��$^ XType here a new simconnect event and hit enter. The event will then be selectable above.��$

 Wrap=�$	��300��
�������
���   ��� $��30��
������ �� $��3��$��8��$ 
Horizontal=�$��Center=�$��Center=�.+��� -6   $��3��$��8��$��80��.��Save�� $��20��
������ �� -7   $��3��$��8��$��80��.��Exit�� $ 
Horizontal=�