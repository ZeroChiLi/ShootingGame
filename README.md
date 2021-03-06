# ShootingGame
- 射击游戏练习，注重游戏设计和Shader特效

## 操作

- 移动：左右方向键
- 冲刺：左Shift键
- 跳跃：空格键
- 射击：X键

## 游戏设计

- Doc/30个让游戏更有感觉的小技巧.md



### 待做

- 跟踪弹
- 连锁闪电
- 尸体持续流血



## Shader

- 人物受击白光
  - 原理：渲染多一遍角色（Graphics.DrawMesh），只渲染指定颜色
  - 文件："Assets/Shader/OnlyColor.shader"; "Assets/Scripts/Effect/ColorEffect.cs"
- 尸体（子弹壳）消融
  - 参考：https://blog.csdn.net/puppet_master/article/details/72455945
  - 原理：噪声纹理，低于阈值的直接剔除；接近阈值的，做颜色插值，渐变效果
  - 文件："Assets/Shader/DissolveEffect.shader"; "Assets/Scripts/Effect/GoEffect/GoDissolveEffect.cs"
- 震荡波动（子弹碰撞）

  - 原理：球体通过GrabPass获取屏幕纹理，读uv时，加上球体法线外扩，噪声纹理偏移；缩放，透明
  - 文件："Assets/Shader/BlastWaveEffect.shader"; "Assets/Scripts/Effect/GoEffect/GoBlastWaveEffect.cs"
- 震荡波动2D（开枪火花）
  - 原理：读纹理，偏移GrabPass
  - 文件："Assets/Shader/BlastWave2DEffect.shader"; 
- 遮挡高亮（敌人被墙体挡住）
  - 原理：多渲染一次，不写入深度就好了
  - 文件："Assets/Shader/OcclusionHighLight.shader"; 
- 遮挡消融（玩家被墙体挡住）
  - 参考：https://blog.csdn.net/puppet_master/article/details/73478905
  - 原理：根据相对屏幕位置和对象距离，直接对墙体做消融即可
  - 文件："Assets/Shader/OcclusionDissolve.shader"; 
- 残影（冲刺幻影）
  - 原理：用`CombineInstance`合并所有子网格，每次更新设置一下转换矩阵调整位置，然后调用DrawMesh渲染这个网格。
  - 文件："Assets/Scripts/Effect/GoEffect/GoGhostEffect.cs"
- 能量护盾（敌人概率自爆冲击波）
  - 参考：https://zhuanlan.zhihu.com/p/35922906
  - 原理：边缘高亮，相交高亮（计算场景深度 和 自身坐标深度比较 如果相近 说明接近相交处）
  - 文件："Assets/Shader/TransparentShield.shader"
- 击中弹痕
  - 参考：Unity官方粒子特效Demo
  - 原理：射线命中点创建贴花特效，设置朝向为命中点法线方向。
- 血迹贴花
  - 参考：https://forum.unity.com/threads/how-do-they-do-the-painting-in-splatoon.460663/
  - https://blog.csdn.net/puppet_master/article/details/84310361
  - https://forum.unity.com/threads/released-dynamic-decals.450820/
  - 原理：根据深度将坐标投影
  - 文件："Assets/Shader/DeferredDecal.shader"
- 屏幕空间的环境光遮蔽 SSAO
  - 参考：https://blog.csdn.net/puppet_master/article/details/82929708
  - 原理：
    1. 采样点的半球范围内随机采样点（正态分布），采样点转到屏幕空间，计算当前深度和采样点深度，如果多数采样点深度大于自己的点，说明该点遮蔽多。
    2. 注意深度相差大于半径的时候，就不算遮蔽，应该是处于边缘。
    3. 双边滤波高斯模糊，高斯模糊的核 + 法线差别大的保留细节。用来保留细节同时又高斯模糊处理遮蔽颜色。
  - 文件："Assets/Shader/ScreenSpaceAOEffect.shader"

### 待做

- 无敌1边缘光：
  - 参考：https://blog.csdn.net/puppet_master/article/details/53548134
- 草地交互

  - 参考：https://zhuanlan.zhihu.com/p/74726921
    - https://blog.csdn.net/qq_33967521/article/details/85205938
    - https://www.cnblogs.com/murongxiaopifu/p/7572703.html
- 体积光

  - 参考：https://blog.csdn.net/puppet_master/article/details/79859678 
    - https://zhuanlan.zhihu.com/p/31618992
  - 原理：
    - 方法1：广告牌粒子
    - 方法2：特殊网格（法线朝向类似光照方向），位置外扩法线方向渐变颜色
    - 方法3：获取光源，顶点位置延光源方向外扩。
    - 方法4：屏幕后处理：Boom光照+径向模糊。
- 体积阴影

  - 参考：https://zhuanlan.zhihu.com/p/35553274
- 死后画面黑白
- 动态色环 
  - 参考：https://zhuanlan.zhihu.com/p/34467034
- 水体
  - https://blog.csdn.net/mobilebbki399/article/details/50493117
- 玻璃
- 尸体四溅：ShatterTootKit
  - 原理：
    1. 搞一个平面：给定一个法线和一个原点可以创建出来
    2. 判断模型所有顶点，原点到所有顶点的向量 点乘 平面法线可以分开平面前后两个顶点列表
    3. 遍历三角形，如果三个点不在同一个部分中，那要切割一下
       1. 三角形边和三角形点到原点的向量 相除可以切割交点
    4. 两层遍历把所有切割相邻最近的连一起
    5. 填充截面
- 屏幕扭曲
  - 参考：https://blog.csdn.net/puppet_master/article/details/71437031
- 冲刺屏幕径向模糊
  - 参考：https://blog.csdn.net/puppet_master/article/details/54566397
- 天气（雨天）
- 体积云
  - 参考：https://blog.csdn.net/qq_33967521/article/details/102657017
  - http://walkingfat.com/bump-noise-cloud-3d%E5%99%AA%E7%82%B9gpu-instancing%E5%88%B6%E4%BD%9C%E5%9F%BA%E4%BA%8E%E6%A8%A1%E5%9E%8B%E7%9A%84%E4%BD%93%E7%A7%AF%E4%BA%91/
- 通透材质
  - 参考：http://walkingfat.com/simple-subsurface-scatterting-for-mobile-%EF%BC%88%E4%B8%80%EF%BC%89%E9%80%9A%E9%80%8F%E6%9D%90%E8%B4%A8%E7%9A%84%E6%AC%A1%E8%A1%A8%E9%9D%A2%E6%95%A3%E5%B0%84/
- Boom
- 卡通风格
- 分层背景
- https://blogs.unity3d.com/2015/02/06/extending-unity-5-rendering-pipeline-command-buffers/?_ga=2.258486025.226082408.1591519027-1097127804.1537454193

