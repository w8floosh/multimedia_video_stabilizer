   M S B A M L   `   `   `  ����E  AMultimedia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Multimedia
Multimedia  5   	   U/clr-namespace:Multimedia.CustomMarkupExtensions!Multimedia.CustomMarkupExtensions  5   	   E'clr-namespace:Multimedia.CustomControlsMultimedia.CustomControls  5   	        Multimedia.VideoStabilizer   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Multimedia     5   	   ?ext/clr-namespace:Multimedia.CustomMarkupExtensions     5   	   :custom'clr-namespace:Multimedia.CustomControls     5   	     � Title$  Video Stabilizer��5
   	    � SizeToContent$ WidthAndHeight=�6"    � WindowStyle$ ThreeDBorderWindow=�6A    � WindowStartupLocation$ CenterScreen=�6b    � 	Resources 5      ��%    5      .��5      � -   5         �Name$ layout��5      $��0,0,0,0q�6   ���	#FF5D0040 � RowDefinitions 5   
   �� 5      $��32 �6   �� 5      $��16 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��24 �6   �� 5      $��32 �6   �� 5      $	��360 �6   �� 5      $��52 �6   �� 5      $��12 �6   �� 5      $��48 �6   �� 5      $��16 �6    � ColumnDefinitions 5    
   �� 5!      �� 5"      $	��100 �6   �� 5#      $	��440 �6   �� 5$      $	��100 �6   �� 5%      $��32 �6   �� 5&      $	��100 �6   �� 5'      $	��440 �6   �� 5(      $	��100 �6   �� 5)      .+�5+   
   �� -   5         ��Name$ StabilizeButton��5+      $��Stabilize video��6<   $��9��6V   $��22(�6j   {�6x   �� *Ready+� 5,   
   -   5        $��StabilizingProgressBar��5,      $��9��64   $��1��6H   �� 5-   
   $��1��6   $
��Left=�6$   $��2��6?   $
��Wrap=�6L   $	��Top=�6`   $��29��6x   $	��540��6�   $��2��6�   $��20(�6�   
�������$��Input video path��6�   	 � Cursor$	 IBeam��6�   �� 5.   
   -   5        $��InputPathTextBox��5.      $��1��6*   $��2��6:   $��Center=�6N   $��3��6k   
 �� TextWrapping$

 Wrap=�6x   $�� ��6�   $��Center=�6�   $	��552��6�   $��32��6�   $	 IBeam��6�   $��Center=�6�   $��	16,0,32,0q�6�   $��14(�6   �� 
IsReadOnly$
 True��6   �� IsUndoEnabled$ False��6,  �� 5/   
   -   5        $��3��5/      $��Select file��6!   $��Center=�67   $��32��6T   $��3��6`   $��Center=�6m   $	��106��6�   $��16(�6�   �� 50   
   $��5��6   $
��Left=�6$   $��2��6?   $
��Wrap=�6L   $��Center=�6`   $��32��6{   $	��540��6�   $��2��6�   $��20(�6�   
�������$:��4Keypoint detection algorithm for video stabilization��6�   �� 51   
   -   5        $��AlgorithmSelector��51      $��5��6,   $��3��6<   $��Center=�6I   $��32��6d   $��2��6p    �� SelectedIndex$ 0��6�   $��16(�6�   ��52      ��  �� Source B   <Multimedia.CustomMarkupExtensions.EnumBindingSourceExtension  *M� *local:KPDAlgorithm++�� 54   
   $��1��6   $
��Left=�6$   $��5��6?   $
��Wrap=�6L   $��Center=�6`   $��32��6{   $	��540��6�   $��2��6�   $��20(�6�   
�������$��Output video path��6�   �� 55   
   -   5        $��OutputPathTextBox��55      $��1��6+   $��2��6;   $��Center=�6O   $��6��6l   $

 Wrap=�6y   $��Center=�6�   $	��552��6�   $��32��6�   $	 IBeam��6�   $��Center=�6�   $��	16,0,32,0q�6�   $��14(�6  �� 56   
   -   5        $��3��56      $��Choose path��6!   $��Center=�67   $��32��6T   $��6��6`   $��Center=�6m   $	��106��6�   $��16(�6�   �� 57   
   $��5��6   $
��Left=�6$   $��5��6?   $
��Wrap=�6L   $��Center=�6`   $��32��6{   $	��540��6�   $��2��6�   $��20(�6�   
�������$3��-Video stabilization quality assessment metric��6�   �� 58   
   -	   5        $��MetricSelector��58      $��5��6)   $��2��69   $��6��6M   $��Center=�6Z   $��32��6u   $��16(�6�   $ 0��6�   ��59      ��    *M� *local:VSQAMetric+++   %Multimedia.CustomControls.VideoPlayer  5;   
   -
   5          Name$ NotStabilizedVideoPlayer��5;      $��1��6?   $��9��6O   $��2��6\   $��3��6m     5<   
   -   5        $ StabilizedVideoPlayer��5<      $��5��6<   $��9��6L   $��3��6Y   $��2��6m   �� 5>   
   $��2��6   $��Center=�6$   $��8��6A   $
��Wrap=�6N   $��Center=�6b   $��32��6}   $	��440��6�   $��20(�6�   
������� �� TextAlignment$ Center=�6�   �� 6�    � Language$ it-it�6�   $�Not stabilized��6�   6�   �� 5?   
   $��6��6   $��Center=�6$   $��8��6A   $
��Wrap=�6N   $��Center=�6b   $��32��6}   $	��440��6�   $��20(�6�   
�������$ Center=�6�   �� 6�   $ it-it�6�   $�
Stabilized��6�   6�   �� 5@   
   $��4��6   $��Center=�6$   $	��444��6A   $��3��6N   $��8��6_   $��Center=�6l   $��12��6�   ���	#FFC8C8C8 J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5A      V� 5B      .~�5C      �� �� 5D      �� 5E       �� Angle$ 0Q�6&   T� 5F      5G      5I      �� 5J   
   $��12��6   $��7��6    ���	#FFC8C8C8$��9��6>   �� 5K   
   $��12��6   $��11��6    ���	#FFC8C8C8$��9��6?   �� 5L   
   -   5        $��7��5L   2   $��Plot trajectory��6B   $��Center=�6\   $��0,24,0,0q�6y   $��12��6�   $	��Top=�6�   $	��100��6�   $��24��6�   y�6   �� *	Completed+6�   �� 5M   
   -   5        $��5��5M   2   $��Plot transforms��6B   $��Center=�6\   $��0,24,0,0q�6y   $��12��6�   $	��Top=�6�   $	��100��6�   $��24��6�   y�6   �� *	Completed+6�   �� 5N   
   -   5         ��Name$ MetricInputValue��5N   5   $��2��6O   $��12��6_   $	��440��6m   $��14(�6y   
�������$ Center=�6�   $
��Wrap=�6�   $��0,24,0,0q�6�   $��24��6�   $��MetricInputValue��6�   y�6   �� *	Completed+6�   �� 5O   
   -   5        $ MetricOutputValue��5O   5   $��6��6P   $��12��6`   $	��440��6n   $��14(�6z   
�������$ Center=�6�   $
��Wrap=�6�   $��0,23,0,1q�6�   $��24��6�   $��MetricOutputValue��6�   y�6   �� *	Completed+6�   5P      5Q      