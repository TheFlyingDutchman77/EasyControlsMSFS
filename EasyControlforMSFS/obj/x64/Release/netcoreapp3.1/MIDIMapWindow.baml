   M S B A M L   `   `   `  ���� M  IEasyControlforMSFS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EasyControlforMSFSEasyControlforMSFS  &     EasyControlforMSFS.MIDIMapWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  ,local clr-namespace:EasyControlforMSFS     MaterialDesignBody#���     MaterialDesignPaper#���  $��Medium"�$��14(�  � 
ResizeMode$  NoResize=� � Title$) #EasyControls for MSFS - MIDI mapper��$	��500��$
��1100�� � Icon$ /flight_30822 (1).png����:  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension  .���  � RowDefinitions �� $��1* ��� $	��12* �.+��� -   $��Header��$��1��$��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $��0,0,0,0q�$��MIDI mapper��$
��Wrap=�$��Center=�$��Center=����	#FBFFFFFF$��20(�� $��1�� � ColumnDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � �� $��2* ��� $��1* ��� $��2* ��� $��1* ��� $��2* ��� $��1* ��� $
��4.5* �.+�A� -   $��	MIDIKnob1��$��40��$��0��$��0��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob2��$��40��$��0��$��1��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob3��$��40��$��0��$��2��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob4��$��40��$��0��$��3��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob5��$��40��$��0��$��4��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob6��$��40��$��0��$��5��
���   �
���   �$��40��$��Center=�$��Center=�A� -   $��	MIDIKnob7��$��40��$��0��$��6��
���   �
���   �$��40��$��Center=�$��Center=�A� -	   $��	MIDIKnob8��$��40��$��0��$��7��
���   �
���   �$��40��$��Center=�$��Center=��� -
   $��MIDIButton8��$��40��$��2��$��0��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton9��$��40��$��2��$��1��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton10��$��40��$��2��$��2��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton11��$��40��$��2��$��3��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton12��$��40��$��2��$��4��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton13��$��40��$��2��$��5��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton14��$��40��$��2��$��6��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton15��$��40��$��2��$��7��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton16��$��40��$��4��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton17��$��40��$��4��$��1��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton18��$��40��$��4��$��2��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton19��$��40��$��4��$��3��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton20��$��40��$��4��$��4��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton21��$��40��$��4��$��5��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton22��$��40��$��4��$��6��
���   �
�������$��40��$��Center=�$��Center=��� -   $��MIDIButton23��$��40��$��4��$��7��
���   �
�������$��40��$��Center=�$��Center=��� -   $��
MIDISlider��$	��200��$��4��$��1��$��8��
���   �
�������$��40��$��Center=�$��Center=��� $��0��$��3��$��6��$ Vertical=�.+��� $ 
Horizontal=�.+��� -   $��LabelACname��$��Profile:��$	��100��$��20,0,0,0q��� -   $��ComboBoxACnames��$
��Left=�$	��200�� �� 
IsEditable$
 True���� $ Vertical=�$��6��$��0��$��0,80,0,0q�.+��� -   $��MIDIButtonA��$��40��
���   �
��� ���$��40��$��Center=�$��Center=��� -   $��LabelA��$��Mode A��$��60��$��	0,000,0,0q�$��Center=�$��Center=��� $ Vertical=�$��6��$��1��$��0,80,0,0q�.+��� -   $��MIDIButtonB��$��40��
���   �
�������$��40��$��Center=�$��Center=��� -    $��LabelB��$��Mode B��$��60��$��	0,000,0,0q�$��Center=�$��Center=��� $��0��$��3��$��6��$ Vertical=�$��0,40,0,0q�.+��� $ 
Horizontal=�.+��� -!   $��LabelActiveProfile��$��Active profile:��$	��100��$��20,0,0,0q��� -"   $!��TextBoxCurrentActiveProfile�� � ToolTip$, &Type here the event when right rotated��$�� �� �� TextWrapping$
 Wrap=�$	��200��
��������� $��3��$��4��$��6��$ Vertical=�.+��� $ 
Horizontal=�.+��� -#   $��
LabelEvent��$��	Event ON:��$	��140���� -$   $��TextBoxMIDIEventOn��$1 +Type here the event for pressing the button��$�� ��$
 Wrap=�$	��300��
�������$��12(�
���   ��� $ 
Horizontal=�.+��� -%   $��LabelEventOFF��$��
Event OFF:��$	��140���� -&   $��TextBoxMIDIEventOFF��$8 2Type here the event for when button is pressed OFF��$�� ��$
 Wrap=�$	��300��
�������$��12(�
���   ��� $ 
Horizontal=�.+��� -'   $��LabelStatusVar��$��Status variable:��$	��140���� -(   $��TextBoxMIDIStatusVar��$4 .Type here the read event to turn button on/off��$�� ��$
 Wrap=�$	��300��
�������$��12(�
���   ��� -)   $��StackPanel_EventLeft��$ 
Horizontal=�$y�Hidden=�.+��� -*   $��LabelEventLeft��$��Event left turn:��$	��140���� -+   $��TextBoxMIDIEventLeft��$+ %Type here the event when left rotated��$�� ��$
 Wrap=�$	��300��
�������
���   �$��12(��� -,   $��StackPanel_EventRight��$ 
Horizontal=�$y�Hidden=�.+��� --   $��LabelEventRight��$��Event right turn:��$	��140���� -.   $��TextBoxMIDIEventRight��$, &Type here the event when right rotated��$�� ��$
 Wrap=�$	��300��
�������
���   �$��12(��� $��7��$��3��$��6��$ Vertical=�.+��� -/   $��ButtonAddProfile��$	��110��$��30��$��	0,20,0,10q�.��Add profile�� -0   $��
ButtonSave��$	��110��$��30��$��0,0,0,10q�.��Save�� -1   $��
ButtonExit��$	��110��$��30��.��Exit