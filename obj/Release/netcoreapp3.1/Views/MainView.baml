   M S B A M L   `   `   `  ���� F  BAnkiPronoun, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Menutest.ViewsMenutest.Views  C&clr-namespace:Menutest.Views.BehaviorsMenutest.Views.Behaviors      Menutest.Views.MainView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:Menutest.Views     4b&clr-namespace:Menutest.Views.Behaviors       � Title$  MainView��$	��450��$	��500��.��]� .+�o� $	��Top=�.@�m� $��	File (_F)��.@�m� -    m�Name$ 
targetmenu��$��Open(_O)�� m� Command �� *OpenFileCommand+3   -Menutest.Views.Behaviors.CommonDialogBehavior   Callback �� *DialogCallback+m� $��Media Directory(_D)�� �� *SelectDirectory+   DirCallback �� *DialogDirCallback+m� $��Help(_H)���� $��Bottom=�� ���	#FFDBEBEA.+��� -    ��Name$ doneBtn��$
��Done��$
��Left=�$��40��$	��Top=�$	��100��$��16(�$��Arial Black)� J� RenderTransformOrigin$ 0.462,-1.489&�$��
70,165,0,0q����� *Readprocess+{��� *IsActiveDone+�� $��Cancel��$
��Left=�$��40��$��285,164,0,0q�$	��Top=�$	��100��$��16(�$��Arial Black)����� *Cancel+�� -    ��Name$ label1��$
��Left=�$��35��$��111,272,0,0q�$	��Top=�$	��152��$ 0.781,1.001&�$��Arial Black)�$��20(����� *Status+�� -   $ label2��$
��Left=�$��34��$��353,228,0,0q�$	��Top=�$	��113��$��20(�$��Arial Black)����� *
Progress+� $
��Left=�$��20��$��
35,235,0,0q�$	��Top=�$	��300�� � Minimum$ 0Q�	 � Maximum$		 100Q�
 � Value
 �� *BarProgress+�� $
��Left=�$��30��$��
23,121,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��450������ *Dir+�� $,��&Directory where media fiiles are saved��$
��Left=�$��30��$��	23,91,0,0q�$	��Top=�$	��369��$��Arial Black)�$��16(��� $
��Left=�$��30��$��	23,46,0,0q�$
 Wrap=�$	��Top=�$	��450������ *Fn+�� $��Target file��$
��Left=�$��30��$��	23,16,0,0q�$	��Top=�$	��380��$��Arial Black)�$��16(�