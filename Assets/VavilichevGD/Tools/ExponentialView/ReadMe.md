# BigNumber Attribute

This asset was developed for making incremental games, where you must use extra large numbers. Asset just shows number in editor in convenient way, using exponential format of number. <br>
Just use BigNumber attribute for double type.


![image](https://user-images.githubusercontent.com/22970240/127550560-fcb4d987-5407-430e-8736-75c1ea6c74d7.png)


You can fill mantissa and exponent in editor. The limits are: <br>
From 1 to 9.99 for mantissa, but you can also put 0;<br>
From -2 to 307 for exponent.<br>
<br>
You cannot write the numbers that are out of this limits, asset automatically clamps it.

![image](https://user-images.githubusercontent.com/22970240/127550783-d9844c2d-1cf7-4c56-8921-08de14067b22.png)


Asset contains translation of numbers. It will be usefull for incremental games that shows you extra large numbers in short format.

![image](https://user-images.githubusercontent.com/22970240/127551175-af73d8ad-e1a4-4619-a3fd-d5314917ca98.png)

You can use simple formatting:

![image](https://user-images.githubusercontent.com/22970240/127552121-e8ebff9b-6e93-4505-87f3-863e5e6ea308.png)


Or specific translation:

![image](https://user-images.githubusercontent.com/22970240/127552188-a60cedae-67bb-4dcf-a54f-4362e48073c6.png)

For specific translation check that specified dictionary of language exists. If not write your own. Watch BigNumberDictionaryEn.cs or BigNumberDictionaryRu.cs for details. Dont forget to add it to BigNumberTranslator.cs class.

![image](https://user-images.githubusercontent.com/22970240/127552728-b0a635de-6dd6-4d04-a26f-2ba822118427.png)
