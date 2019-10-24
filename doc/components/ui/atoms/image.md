# KhImage

## Description

KhImage is inherited from UnityEngine.UI.Image, and is given some useful features.

- mirror
- rotate
- skew

## Usage

1. mirror  

    By setting the mirror parameter, you can make the image flip.  
    It has three options: horizontal, vertical, Both.

2. rotate  

    The rotate parameter can be set to a integer between 0 and 3, which corresponds to the rotation degrees of 0, 90, 180, 270.

3. skew

    This option can make the image skew.
    In order to ensure the click range as much as possible, the skew is around the center, and the max distance of coordinates x and y, will be the same as 2 times the Vector2 value of skew option you set.
