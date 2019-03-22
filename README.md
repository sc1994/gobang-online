# gobang-online
在线五子棋

>### 使用
- 后端运行
```
git clone https://github.com/sc1994/gobang-online.git
cd gobang-online/SocketAPI
dotnet run 
```
> 注意：修改redis链接到你自己的服务
- 页面运行
```
cd gobang-online/SocketAPI/wwwroot/
npm i 
# 访问/SocketAPI/wwwroot/index.html 文件
```
> 注意：修改`baseUrl`、`socketUrl`到你自己的后台

>### 邀请
- 分享自己的链接 `xxxx/wwwroot/index.html?room=xxxxxx` 给好友

>### 规则
- 先进房间的执黑
- 除前两人外，其他人观战
- 刷新页面视为退出房间，重新进入

>### todo
- 胜负手之后的验证逻辑
- socket大小优化，延迟优化
- ai逻辑
- 计算逻辑优化

