# Unity3D角色动画系统

### 介绍

>  使用Unity3D引擎和C#语言开发的ARPG游戏Demo，期待作为暑期实习的简历项目，侧重于对Unity动画系统的练习，目前正尽量提高完成度中~
>
> PS：虽然说是ARPG但是越做越像魂游了。。

### 软件架构

- 游戏引擎：Unity3D
- IDE：Rider（很好用👍）

### 参考教程
- B站up主IGBeginner0116：[Unity入门教程，初学者请按顺序观看](https://space.bilibili.com/269749034/channel/collectiondetail?sid=48663&spm_id_from=333.788.0.0)
- B站up主鬼鬼鬼ii：[Unity ARPG开发教程 第一期_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1sB4y1n7hg/?spm_id_from=333.1387.homepage.video_card.click&vd_source=cff6ad0add5a2da19d9fb3ab37296400)
- 油管Sebastian Graves：[HOW TO CREATE DARK SOULS in UNITY](https://www.youtube.com/watch?v=HKMo3pczQyc)

### 第三方资源

- 模型：
    - 后崩坏书护士怪（Boss）
    - 月下誓约（主角）
- 动作：
    - [Mixamo](https://www.mixamo.com/)
    - [UnityAssetsStore](https://www.bing.com/search?pglt=299&q=UnityAssetsStore&cvid=36faf28cd5f94567aceba5e069f0988f&gs_lcrp=EgRlZGdlKgYIABBFGDkyBggAEEUYOTIGCAEQABhAMgYIAhAAGEAyBggDEAAYQDIGCAQQABhAMgYIBRAAGEAyBggGEAAYQDIGCAcQABhAMgYICBAAGEDSAQgyOTA1ajBqMagCALACAA&FORM=ANNTA1&adppc=EdgeStart&PC=ACTS)
- 音效：
    - [UnityAssetsStore](https://www.bing.com/search?pglt=299&q=UnityAssetsStore&cvid=36faf28cd5f94567aceba5e069f0988f&gs_lcrp=EgRlZGdlKgYIABBFGDkyBggAEEUYOTIGCAEQABhAMgYIAhAAGEAyBggDEAAYQDIGCAQQABhAMgYIBRAAGEAyBggGEAAYQDIGCAcQABhAMgYICBAAGEDSAQgyOTA1ajBqMagCALACAA&FORM=ANNTA1&adppc=EdgeStart&PC=ACTS)
    - 部分音效来自《黑神话悟空》等游戏及AI
- Shader：
    - GitHub [SimpleURPToonLitExample](https://github.com/ColinLeung-NiloCat/UnityURPToonLitShaderExample?tab=MIT-1-ov-file)
- *还有很多没有此处没有提到的资源，因为用到的资源太多太杂了；总之大部分资源来自Unity的资源商店，在上面多搜一搜就能找到效果很好的*

### 开发日记

#### 2024.12.09-2024.12.15 提交1

##### 新增功能

- 新建了项目文件夹:)
- 基本完成了基础动画和状态机的调整（后续可能会持续优化）
- 手写了基础的代码控制地面检测
- 调整了第三人称摄像机的基础参数
- 完成了基础的移动、跳跃、下蹲等动作的代码控制
- 新增了两个表情动画“问好”和“赞同”
- 为脚步声和跳跃添加了基础的音效（脚步声音效后期应该还会修改）
##### 待修复Bug
- [x] 角色在贴墙时跳跃会发生异常的下蹲动作切换，且该切换疑似会打断跳跃动作(*12.15已解决*)
- [x] 角色在上坡时跳跃会导致播放两次跳跃音效（应该是因为上坡时跳跃的地面检测造成的）(*12.15已解决*)
<br>

#### 2024.12.15 提交2

##### 新增功能

- 修改了角色移动速度
- 完成了角色跳跃需要用到的地形检测功能脚本的编写
- 新增了角色语音
##### 待修复Bug
- 解决了**提交1**中的**待修复Bug**
- 暂无新的Bug

<br>



#### 2024.12 - 2025.03

> 中间放寒假的时候刷力扣去了T T，后面二月多的时候在准备暑期实习的面试，基本没怎么做这个项目

<br>



#### 2025.3.03

##### 新增功能

- 重构了第三人称控制器代码，解耦战斗状态和普通状态
- 完善切换武器的逻辑
- 新增攻击逻辑

> 我发现我真的做不到每次push都写一次readme，所以可能以后就像本次记录一样，多次push记录一次吧！

<br>



#### 2025.3.07

##### 新增功能

- 新增敌人AI视野
- 修复敌人锁定的一些Bug
- 新增角色攻击的射线攻击检测

<br>



#### 2025.3.15 - 2025.3.16

##### 新增功能

- 重构了连招系统，实现配置可视化和可拓展，并增加了攻击音效、特效、受击特效
- 重新写了可视化可拓展可配置的有限状态机写法（结合SciptableObject）
- 做了一些UI，但还没添加交互逻辑
- 导入了自制框架
- 重构了之前的shit山代码，降低了耦合度

<br>



#### 2025.3.18

##### 新增功能

- 有限状态机的可视化重构
- 新增了敌人AI的移动和普通攻击逻辑
- 导入了地图资源（现在游戏看起来没那么简陋了）

<br>



#### 2025.3.19

##### 新增功能

- 新增敌人AI技能
- 新增了动作素材
- 更换了Boss的模型（换为了后崩坏书的护士怪）
- 新增敌人攻击的音效、特效、攻击检测
- 修复了对象池有时候抛空异常的Bug

<br>



#### 2025.3.20

##### 新增功能

- 新增了玩家受击的逻辑
- 新增大剑砸地的地面特效
- 修复了特效位置不正确的Bug
- 新增了简陋的BGM播放功能

##### 待修复Bug

- 敌人攻击太快导致基于碰撞体的敌人攻击检测不准确，打算修改为和玩家一样的射线检测

<br>



#### 2025.3.21

##### 新增功能

- 新增了敌人射线攻击检测的逻辑
- 新增了敌人技能（目前共5个技能+1个普通攻击）
- 基本完成敌人和玩家的攻击和受击逻辑
- 更换了玩家的受击特效
- 完善了敌人受击特效的效果
- 新增了地图边界和边界碰撞体

<br>



#### 2025.3.22

##### 新增功能

- 新增了敌人模型的布料模拟
- 新增了普通闪避和完美闪避逻辑，以及完美闪避的子弹时间和闪避音效
- 修复了敌人的X轴和Z轴会旋转的Bug
- 修复了玩家在攻击过程中受击，攻击检测会一直开启的Bug

<br>



#### 2025.3.23

##### 新增功能

- 新增了锁定目标状态下的大剑、太刀的移动、闪避动画
- 重构了Animator动画状态机，减少了一些蜘蛛网🕸和重复的转换条件
- 改进了锁定目标状态下面朝敌人的计算逻辑
- 完成了单个锁定目标情况下的锁定功能，包括摄像机、面朝目标的八向移动等等，基本完善~

##### 待修复Bug

- 后面如果有时间就改进一下多个目标情况下的锁定，使用一个数组来存储待切换目标即可（但因为现在的逻辑问题这么写需要重构，现在尽量先把基本功能做完）

<br>



