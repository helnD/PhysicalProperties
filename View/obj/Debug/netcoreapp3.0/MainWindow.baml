   M S B A M L   `   `   `  ����?  ;View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ViewView  5   	       View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:View     5   	     � WindowStyle$
  None=�5   	   $	��650��6   $��645.5��6)    � 
ResizeMode$ NoResize=�67   ���	#FF211438.��5	      � .+�5
   
   � -   5        $��30��5
      $	��Top=�6   .+�5      �� $R��LФизические свойства объекта и маркировка��6   $��0,0,0,0q�6G   $	��606��6X   $��10,6,6,0q�6d   $
��Left=�6w   ��5      �� 5       �� Color$ 	#FFD8D8D8��6&   5      �� 5      -   5         ��Name$ CloseButton��5         CloseButtonStyle#���   6*   $��30��6a   $��40��6m   $��0,0,0,0q�6x   $	��Top=�6�   $��Right=�6�   $��Center=�6�   ��6�   Z� ��6�   Z� .��5      �� $��12��6   $��12��6$   $��res/img/cross.png��6/   5      5      �� 5   
   -   5         ��Name$ 
DrawCanvas��5      $
��Left=�6%   $	��375��6@   $��	48,46,0,0q�6M   $	��Top=�6`   $	��375��6x   
������� J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ 	#FF464646��6#      
BlurRadius$ 10Q�65   5      �� 5   
   $6��0Метод маркировки объектов��6   $��
440,54,0,0q�64   $��28��6H   $	��Top=�6T   ���	#FFD8D8D8$
��Left=�6�   $	��206��6�   6�   	� 5   
   -   5         	�Name$ LineMethodRadioButton��5      $��
460,89,0,0q�65   	 d� 	IsChecked$
	 TrueY�6I   $��Построчный��6�   $��27��6�   $	��Top=�6�   ���	#FFD8D8D8$
��Left=�6�   $	��176��6�   6�   	� 5   
   -   5        $  RecursiveMethodRadioButton��5      $��460,114,0,0q�6:   $��Рекурсивный��6~   $��26��6�   $	��Top=�6�   ���	#FFD8D8D8$
��Left=�6�   $	��176��6�   6�   �� 5   
   -   5        $ ClearCanvasButton��5        ClearButtonStyle#���  6,   $��Очистить��6�   $
��Left=�6�   $��450,183,0,0q�6�   $	��Top=�6�   $	��152��6�   ���	#FFD8D8D86�   �� 5   
   -   5        $ CalculateButton��5        ButtonBaseStyle#���  6*   $��Посчитать��6T   $
��Left=�6h   $��450,154,0,0q�6�   $	��Top=�6�   $	��152��6�   ���	#FFD8D8D86�   ,� 5   
   $��	0,441,0,0q�6#   $
 ,� HorizontalScrollBarVisibility$
 Disabled=�66   " ,� VerticalScrollBarVisibility$
 Auto=�6_    ,� SelectionBrush �	#FFC14646���	#FFD8D8D8$��Center=�6�   $��Center=�6�   ��6�   Z� .��5      /� .T�5       �� -   5         �Name$ 	DataTable��5        �� CellSpacing$ 0��6)    �� BorderThickness$ 0q�69   $��Segoe UI)�6M   $��12(�6c    �� BorderBrush �	#FFDADADA �� Columns 5!      �� 5"       �� 
Background
 ����  �� Width$ 30 �6?   �� 5#      
 ���� $ 70 �6?   �� 5$      
 ���� $ 90 �6?   �� 5%      
 ���� $ 90 �6?   �� 5&      
 ���� $	 100 �6?   �� 5'      
 ���� $	 100 �6?   �� 5(      
 ���� $	 120 �6?   5*      5+      5,      5-      5.      