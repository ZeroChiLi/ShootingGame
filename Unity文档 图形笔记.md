# Unity文档 图形笔记

## Unity内置渲染管道

> [Unity Shader 学习笔记（12） 渲染路径（Rendering Path）](https://blog.csdn.net/l773575310/article/details/78569537)
>
> [Unity Shader 学习笔记（13） 混合光源、光的衰减](https://blog.csdn.net/l773575310/article/details/78572067)

### 前向渲染

- 文档：https://docs.unity3d.com/Manual/RenderTech-ForwardRendering.html

- 描述：渲染所有对象一次或多次pass（受n个光照影响就n个pass），光照处理方式根据本身不同设置和强度。
  - 对象影响最大（距离、强度）的4个光源依次计算每个顶点，其他通过球谐函数（Spherical Harmonics (SH)）模拟。

- 一个光是否为每像素光取决于以下几点：
  - 渲染模式设置为不重要的灯光总是逐顶点或SH。
  - 最亮的定向光总是逐像素的。
  - 渲染模式设置为重要的灯光总是逐像素。
  - 如果以上的结果比当前像素光计数（[Quality Setting](https://docs.unity3d.com/Manual/class-QualitySettings.html)中设置）的光照更少，那么更多的光照逐像素渲染，按照亮度降低的顺序。

#### 渲染过程

- Base Pass 处理一个逐像素定向光和所有逐顶点/SH光。
- Additional Passes 渲染其他逐像素的光，一个光源一个pass。

> ![img](https://docs.unity3d.com/uploads/Main/ForwardLightsExample.png)
>
> - 假设A~H光照颜色强度等所有配置相同：最亮的灯光将以逐像素光照模式(A到D)进行渲染，然后最多4个灯光以逐顶点光照模式(D到G)进行渲染，最后以SH (G到H)进行渲染。
>
> ![img](https://docs.unity3d.com/uploads/Main/ForwardLightsClassify.png)

- 默认BassPass 支持阴影，AdditionalPasses要阴影需要用Shader变体关键字[multi_compile_fwdadd_fullshadows](https://docs.unity3d.com/Manual/SL-MultipleProgramVariants.html)。

#### 球谐光照（Spherical Harmonics (SH)）

球谐光照的渲染速度非常快。在CPU上的成本很小，可真正自由供GPU应用（也就是说，BassPass总是计算SH照明；但是由于SH照明的工作方式，无论有多少SH照明，成本都完全相同）。

SH的缺点是：

- 它们是在对象的顶点处计算的，而不是在 **像素**，这表示他们不支持轻度Cookie或**法线贴图**
- SH光照频率很低。SH灯不能有尖锐的灯光过渡。它们也仅影响漫射照明（对于高光高光而言频率太低）。
- SH光照计算不是local的；靠近某个表面的点或点SH光照将“看起来不正确”。

### 延迟渲染

- 文档：https://docs.unity3d.com/Manual/RenderTech-DeferredShading.html
- 描述：没有限制作用于对象的光照数量，光照的处理开销与光照的数量成正比，所有光照都是逐像素的，光照是在屏幕空间中计算的，和场景复杂度无关。
  - 仍然需要为每个阴影投射光渲染一次或多次阴影投射对象。
- 缺点：不支持抗锯齿（anti-aliasing），不能处理半透明物体，不支持`Mesh Renderer`的`Receive Shadows`开关，相机剔除遮罩（`culling masks`）支持有限，最多支持4个剔除遮罩。
- 要求：不支持相机的正交投影，正交投影的相机的`falls back`是前向渲染。

#### 渲染过程

1. Base Pass：渲染对象以产生具有**深度**，**法线**和**镜面反射力**的屏幕空间缓冲区。
2. Lighting pass：先前生成的缓冲区用于将光照计算到另一个屏幕空间缓冲区中。
3. Final pass：再次渲染对象。他们获取计算出的光照，将其与颜色纹理结合，并添加任何环境/发射光照。

在此过程完成后，无法处理延迟光照的对象用前向渲染路径处理。

#### 实现细节

- 不支持延迟渲染的对象会在延迟渲染完成后用前向渲染完成

- 下面列出了几何图形缓冲区（g缓冲区）中渲染目标（RT0-RT4）的默认布局。数据类型放置在每个渲染目标的各个通道中。使用的通道显示在括号中。

  - RT0，ARGB32格式：漫反射色`Diffuse`（RGB），遮挡`occlusion`（A）。
  - RT1，ARGB32格式： 镜面颜色`Specular`（RGB），粗糙度`roughness`（A）。
  - RT2，ARGB2101010格式：世界空间法线（RGB），未使用（A）。
  - RT3，ARGB2101010（非HDR）或ARGBHalf（HDR）格式：自发光`Emission` + 光照`lighting `+ 光照贴图`lightmaps` + 反射探针`reflection probes` 缓冲。
  - 深度`Depth`+模板`Stencil` 缓冲。

  因此，默认的`g-buffer`布局为160位/像素（非HDR）或192位/像素（HDR）。

  如果将 [Shadowmask](https://docs.unity3d.com/Manual/LightMode-Mixed-Shadowmask.html) 或 [Distance Shadowmask](https://docs.unity3d.com/Manual/LightMode-Mixed-Shadowmask.html) 模式用于混合照明，则使用第五个目标：

  - RT4，ARGB32格式：光遮挡值`Light occlusion`（RGBA）。

  因此，g-buffer缓冲区布局为192位/像素（非HDR）或224位/像素（HDR）。

  如果硬件不支持五个并发渲染目标，则使用阴影遮罩的对象将回退到正向渲染路径。当不使用相机HDR时，自发光+光照缓冲区（RT3）进行了对数编码，以提供比通常使用ARGB32纹理可能提供的更大的动态范围。

  **注意：当相机使用HDR渲染时，不会为自发光+光照缓冲区（RT3）创建单独的渲染目标。而是将Camera渲染到的渲染目标（即传递到图像效果的渲染目标）用作RT3。**

#### G-Buffer Pass

- G-Buffer Pass将每个GameObject渲染一次。漫反射和镜面反射的颜色，表面平滑度，世界空间法线以及自发光+环境光+反射光+光照贴图将渲染为G-Buffer纹理。G-Buffer纹理设置为全局着色器属性，以供着色器（*CameraGBufferTexture0 ..* *CameraGBufferTexture3*）访问。

#### Lighting pass

- 光照 Pass计算基于 G-Buffer 和 深度信息。光照会添加到自发光缓冲区。
- 针对场景启用了Z缓冲区测试的，未穿过相机近平面的点光源和点光源，将被渲染为3D形状，。这使得部分或完全遮挡的点光源和聚光灯渲染起来非常便宜。穿过近平面的方向灯和点/点光源呈现为全屏**四边形**。
- 如果灯光启用了阴影，则还将在此pass中渲染并应用阴影。还需需要渲染阴影投射器（shadow casters），并且必须应用更复杂的灯光着色器。

- 唯一可用的照明模型是`Standrad`。如果需要其他模型，则可以通过将[内置着色器](http://unity3d.com/support/resources/assets/built-in-shaders)的`Internal-DeferredShading.shader`文件的修改后的版本放置到`Resources`文件夹中来修改着色器。然后打开菜单：Edit -> Project Setting -> Graphics。选`Deferred`中 `CustomShader`。

### 使用`CommandBuffer`扩展内置渲染管道

- 文档：https://docs.unity3d.com/Manual/GraphicsCommandBuffers.html
- 某些命令只在指定硬件上支持，例如：光效追踪仅在DX12中支持。
- 相机指令缓冲：使用[Camera.AddCommandBuffer](https://docs.unity3d.com/ScriptReference/Camera.AddCommandBuffer.html) API 和对应枚举 [CameraEvent enum](https://docs.unity3d.com/ScriptReference/Rendering.CameraEvent.html)。
- 光照指令缓冲：使用 [Light.AddCommandBuffer](https://docs.unity3d.com/ScriptReference/Light.AddCommandBuffer.html) API 和对应枚举 [LightEvent enum](https://docs.unity3d.com/ScriptReference/Rendering.LightEvent.html)。

