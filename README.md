# ShootingGame
- 射击游戏练习，注重游戏设计和Shader特效

## 游戏设计

- Doc/30个让游戏更有感觉的小技巧.md
- 冲刺



## Shader

- 人物受击白光
  - 原理：渲染多一遍角色（Graphics.DrawMesh），只渲染指定颜色
  - 文件："Assets/Shader/OnlyColor.shader";"Assets/Scripts/Effect/ColorEffect.cs"

- 尸体（子弹壳）消融
  - 参考：https://blog.csdn.net/puppet_master/article/details/72455945
  - 文件："Assets/Shader/DissolveEffect.shader";"Assets/Scripts/Effect/GoEffect/GoDissolveEffect.cs"

### 待做

- 无敌1边缘光：
  - 参考：https://blog.csdn.net/puppet_master/article/details/53548134

- 能量护盾
  - 参考：https://zhuanlan.zhihu.com/p/35922906
- 震荡波动
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
- 玻璃
- 尸体四溅
- 遮挡高亮
- 血色贴花
  - 参考：https://blog.csdn.net/puppet_master/article/details/84310361
- 屏幕扭曲
  - 参考：https://blog.csdn.net/puppet_master/article/details/71437031
- 冲刺屏幕径向模糊
  - 参考：https://blog.csdn.net/puppet_master/article/details/54566397