   M S B A M L   `   `   `  ���� E  AMultimedia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Multimedia
Multimedia  U/clr-namespace:Multimedia.CustomMarkupExtensions!Multimedia.CustomMarkupExtensions  E'clr-namespace:Multimedia.CustomControlsMultimedia.CustomControls       Multimedia.VideoStabilizer   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:Multimedia     ?ext/clr-namespace:Multimedia.CustomMarkupExtensions     :custom'clr-namespace:Multimedia.CustomControls       � Title$  Video Stabilizer�� � SizeToContent$ WidthAndHeight=� � WindowStyle$ ThreeDBorderWindow=� � WindowStartupLocation$ CenterScreen=� � 	Resources ��%    .��� -    �Name$ layout��$��0,0,0,0q����	#FF5D0040 � RowDefinitions �� $��32 ��� $��16 ��� $��32 ��� $��32 ��� $��32 ��� $��32 ��� $��32 ��� $��24 ��� $��32 ��� $	��360 ��� $��52 ��� $��12 ��� $��48 ��� $��16 � � ColumnDefinitions �� �� $	��100 ��� $	��440 ��� $	��100 ��� $��32 ��� $	��100 ��� $	��440 ��� $	��100 ��� .+��� -    ��Name$ StabilizeButton��$��Stabilize video��$��9��$��22(�{��� *Ready+� -   $��StabilizingProgressBar��$��9��$��1���� $��1��$
��Left=�$��2��$
��Wrap=�$	��Top=�$��29��$	��540��$��2��$��20(�
�������$��Input video path��	 � Cursor$	 IBeam���� -   $��InputPathTextBox��$��1��$��2��$��Center=�$��3��
 �� TextWrapping$

 Wrap=�$�� ��$��Center=�$	��552��$��32��$	 IBeam��$��Center=�$��	16,0,32,0q�$��14(� �� 
IsReadOnly$
 True�� �� IsUndoEnabled$ False���� -   $��3��$��Select file��$��Center=�$��32��$��3��$��Center=�$	��106��$��16(��� $��5��$
��Left=�$��2��$
��Wrap=�$��Center=�$��32��$	��540��$��2��$��20(�
�������$:��4Keypoint detection algorithm for video stabilization���� -   $��AlgorithmSelector��$��5��$��3��$��Center=�$��32��$��2�� �� SelectedIndex$ 0��$��16(�����  �� Source B   <Multimedia.CustomMarkupExtensions.EnumBindingSourceExtension  *M� *local:KPDAlgorithm++�� $��1��$
��Left=�$��5��$
��Wrap=�$��Center=�$��32��$	��540��$��2��$��20(�
�������$��Output video path���� -   $��OutputPathTextBox��$��1��$��2��$��Center=�$��6��$

 Wrap=�$��Center=�$	��552��$��32��$	 IBeam��$��Center=�$��	16,0,32,0q�$��14(��� -   $��3��$��Choose path��$��Center=�$��32��$��6��$��Center=�$	��106��$��16(��� $��5��$
��Left=�$��5��$
��Wrap=�$��Center=�$��32��$	��540��$��2��$��20(�
�������$3��-Video stabilization quality assessment metric���� -	   $��MetricSelector��$��5��$��2��$��6��$��Center=�$��32��$��16(�$ 0������    *M� *local:VSQAMetric+++   %Multimedia.CustomControls.VideoPlayer  -
     Name$ NotStabilizedVideoPlayer��$��1��$��9��$��2��$��3��  -   $ StabilizedVideoPlayer��$��5��$��9��$��3��$��2���� $��2��$��Center=�$��8��$
��Wrap=�$��Center=�$��32��$	��440��$��20(�
������� �� TextAlignment$ Center=���  � Language$ it-it�$�Not stabilized���� $��6��$��Center=�$��8��$
��Wrap=�$��Center=�$��32��$	��440��$��20(�
�������$ Center=��� $ it-it�$�
Stabilized���� $��4��$��Center=�$	��444��$��3��$��8��$��Center=�$��12�����	#FFC8C8C8 J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$ 0Q�T� �� $��12��$��7�����	#FFC8C8C8$��9���� $��12��$��11�����	#FFC8C8C8$��9���� -   $��7��$��Plot trajectory��$��Center=�$��0,24,0,0q�$��12��$	��Top=�$	��100��$��24��y��� *	Completed+�� -   $��5��$��Plot transforms��$��Center=�$��0,24,0,0q�$��12��$	��Top=�$	��100��$��24��y��� *	Completed+�� -    ��Name$ MetricInputValue��$��2��$��12��$	��440��$��14(�
�������$ Center=�$
��Wrap=�$��0,24,0,0q�$��24��$��MetricInputValue��y��� *	Completed+�� -   $ MetricOutputValue��$��6��$��12��$	��440��$��14(�
�������$ Center=�$
��Wrap=�$��0,23,0,1q�$��24��$��MetricOutputValue��y��� *	Completed+