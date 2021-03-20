﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLock : MonoBehaviour
{

    public int target = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }

    void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }
}
/*           .e$.                           z$$e.d$$$.      z$b   z
         d$" .d                        .$$" d"F ^*$$$e  z$" $ .$
       e$P   $%                       d$P ."  F    "$$"d$  .e$"
      $$F                           .$$"  F  J       "$$z$$$"
    .$$"   .$"3   .$""  .$P $$  $$ 4$$"  $  4"       $$  .
   .$$F   d$  4  d$ d$ z$" J$%    4$$"   $.d"       $$  ."
   $$P   $$ ".$z$$  ^ z$" .$P    .$$F              $$" ."
  $$$   d$F  J $$F   z$$  $$ .   $$$   ze     .c  J$F z  .e.ze
 4$$F   $$  4" $$   z$$  $$"."  d$$  d$" $  z$" $ $$ @  $$".$F
 $$$   4$$.d" 4$$ .$3$$.$$$e%   $$P J$P  P d$*  %$$$"  $$  $$
 $$$    $$*    $$$" ^$$"'$$    4$$% $$" . d$" "$"$$   $$  $$
 $$$                           $$$  "   P4$P  z 4$F  $$" J$% %
 '$$c          .e$$$$$$$$e     $$$F    $ $$  z" $$  $$$ 4$$ P
  "$$b.   .e$*"     "$$$$$$$   '$$$c.dP  $$$$"  $$$"$$$$$$$P
    "*$$$*"           "          *$$*"   "$*    "$" ^$* "$"
Я попил ванильной колы))))
*/