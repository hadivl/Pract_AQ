   M S B A M L   `   `   `  ����\  XАквариум_практика, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:Аквариум_практика.Edit&Аквариум_практика.Edit  5   	   ?    9Аквариум_практика.Edit.EditCleaningDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Flocal4clr-namespace:Аквариум_практика.Edit     5   	     � Title$O  IРедактирование оборудования для уборки��5   	   $	��450��68   $	��800��6E   .��5	      �� $
��Auto=�6   .��5
   
   � $��10q�6   $	��254��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $&�� Выкачивание воды:��6   $��0��60   $��0��6=   $��0,0,5,5q�6M   �� 5      -   5         ��Name$ txtPumpingOutWater��5      $��0��62   $��1��6?   $��0,0,0,5q�6O   �� 5       $��Залив воды:��6   $��1��6*   $��0��67   $��0,5,0,5q�6G   �� 5!      -   5        $ txtWaterInlet��5!      $��1��6-   $��1��6:   $��0,5,0,5q�6J   �� 5#      $��Посадка рыб:��6   $��2��6+   $��0��68   $��0,5,0,5q�6H   �� 5$      -   5        $ txtEquipPlantingFish��5$      $��2��64   $��1��6A   $��0,5,0,5q�6Q   �� 5&      $��Для уборки:��6   $��3��6*   $��0��67   $��0,5,0,5q�6G   �� 5'      -   5        $ txtCleaning��5'      $��3��6+   $��1��68   $��0,5,0,5q�6H   �� 5)      $��Для ловли рыб:��6   $��4��6-   $��0��6:   $��0,5,0,5q�6J   �� 5*      -   5        $ txtEquipCatchingFish��5*      $��4��64   $��1��6A   $��0,5,0,5q�6Q   �� 5,      $��Аквариум:��6   $��5��6(   $��0��65   $��0,5,0,5q�6E   �� 5-      -   5         ��Name$ 
cbAquarium��5-      $��5��6+   $��1��68   $��0,5,0,5q�6H   �� 5/      $��9��6    �� Orientation$ 
Horizontal=�6&   $
��Left=�6?   $��
51,10,0,10q�6Z   $��1��6n   .+�50      �� -   5        $��Сохранить��50      $��80��6F   �� 51      -   5        $��Отмена��51      $��80��6E   $��10,0,0,0q�6S   52      53      54      55      