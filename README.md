# ShootingGame
- 射击游戏练习，注重游戏设计和Shader特效

## 游戏设计

- Doc/30个让游戏更有感觉的小技巧.md



## Shader

- 人物受击白光
  - 原理：渲染多一遍角色（Graphics.DrawMesh），只渲染指定颜色
  - 文件："Assets/Shader/OnlyColor.shader"; "Assets/Scripts/Effect/ColorEffect.cs"
- 尸体（子弹壳）消融
  - 参考：https://blog.csdn.net/puppet_master/article/details/72455945
  - 原理：噪声纹理，低于阈值的直接剔除；接近阈值的，做颜色插值，渐变效果
  - 文件："Assets/Shader/DissolveEffect.shader"; "Assets/Scripts/Effect/GoEffect/GoDissolveEffect.cs"
- 震荡波动

  - 原理：球体通过GrabPass获取屏幕纹理，读uv时，加上球体法线外扩，噪声纹理偏移；缩放，透明
  - 文件："Assets/Shader/BlastWaveEffect.shader"; "Assets/Scripts/Effect/GoEffect/GoBlastWaveEffect.cs"
- 遮挡高亮
  - 原理：多渲染一次，不写入深度就好了
  - 文件："Assets/Shader/OcclusionHighLight.shader"; 
- 遮挡消融
  - 参考：https://blog.csdn.net/puppet_master/article/details/73478905
  - 原理：根据相对屏幕位置和对象距离，直接对墙体做消融即可
  - 文件："Assets/Shader/OcclusionDissolve.shader"; 



### 待做

- 冲刺
- 无敌1边缘光：
  - 参考：https://blog.csdn.net/puppet_master/article/details/53548134
- 能量护盾
  - 参考：https://zhuanlan.zhihu.com/p/35922906
- 草地交互

  - 参考：https://zhuanlan.zhihu.com/p/74726921
- 体积光

  - 参考：https://blog.csdn.net/puppet_master/article/details/79859678 
    - https://zhuanlan.zhihu.com/p/31618992
- 体积阴影

  - 参考：https://zhuanlan.zhihu.com/p/35553274
- 死后画面黑白
- 动态色环 
  - 参考：https://zhuanlan.zhihu.com/p/34467034
- 水体
  - https://blog.csdn.net/mobilebbki399/article/details/50493117
- 玻璃
- 尸体四溅
- 血色贴花
  - 参考：https://blog.csdn.net/puppet_master/article/details/84310361
- 屏幕扭曲
  - 参考：https://blog.csdn.net/puppet_master/article/details/71437031
- 冲刺屏幕径向模糊
  - 参考：https://blog.csdn.net/puppet_master/article/details/54566397
- 幻影（运动模糊）
- 天气（雨天）