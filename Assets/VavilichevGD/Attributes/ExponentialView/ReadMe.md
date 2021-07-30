# ExponentialView Attribute

This asset was developed for making incremental games, where you must use extra large numbers. Asset just shows number in editor in convenient way, using exponential format of number. <br>
Just use ExponentialView attribute for double type.


![image](https://user-images.githubusercontent.com/22970240/127616934-6f899e2b-3d67-48f5-8a7c-5852d6e8a150.png)


You can fill mantissa and exponent in editor. The limits are: <br>
From 1 to 9.99 for mantissa, but you can also put 0;<br>
From -2 to 307 for exponent.<br>
<br>
You cannot write the numbers that are out of this limits, asset automatically clamps it.

![image](https://user-images.githubusercontent.com/22970240/127616990-2f79a03d-b109-4b13-8ee6-86338cda7959.png)


Asset contains translation of numbers. It will be usefull for incremental games that shows you extra large numbers in short format.

![image](https://user-images.githubusercontent.com/22970240/127551175-af73d8ad-e1a4-4619-a3fd-d5314917ca98.png)

You can use simple formatting:

![image](https://user-images.githubusercontent.com/22970240/127552121-e8ebff9b-6e93-4505-87f3-863e5e6ea308.png)


Or specific translation:

![image](https://user-images.githubusercontent.com/22970240/127552188-a60cedae-67bb-4dcf-a54f-4362e48073c6.png)

For specific translation check that specified dictionary of language exists. If not write your own. Watch ExponentialViewDictionaryEn.cs or ExponentialViewDictionaryRu.cs for details. Dont forget to add it to ExponentialViewTranslator.cs class.

![image](https://user-images.githubusercontent.com/22970240/127617090-9dbe8684-1c00-463d-ba00-32e9b6bd7ff8.png)
