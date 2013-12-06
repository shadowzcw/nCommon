关于nCommon
=======
nCommon是我2013年在ianylearn团队web开发中沉淀和积累的常用类库，虽然现在ianylearn项目已暂停.<br/>
每个目录为一个知识点.
  

Auth目录
-------
form认证相关<br/>

    AjaxAuthorizeAttribute：ajax请求认证过滤器
    CrossDomainRedirectModule：跨子域form认证跳转returnUrl格式化module
  
Exception目录
-------
异常错误处理相关<br/>

    ApplicationErrorHandle：应用程序全局异常错误处理
    CustomHandleErrorAttribute：异常错误处理过滤器
  
File目录
-------
文件操作相关<br/>

    IUpload：文件上传接口
    NFSUploader：NFS上传实现
    FTPUploader：FTP上传实现
  
IOC目录
-------
### Castle目录
基于Castle的IOC和AOP封装<br/>

    1.IOC
    IOC：IOC容器类，提供类型注册
    RepoInstaller：Repository类注册器
    ControllersInstaller：控制器注册器
    WindsorControllerFactory：Windsor实现的MVC控制器工厂
    2.AOP
    AspectFacility：Castle AOP实现类(采用动态代理)
    MethodBoundaryInterceptor：Castle动态代理实现类(采用拦截器)
    MethodBoundaryAttribute：自定义拦截器基类
    MethodExecutionArgs：自定义拦截器执行上下文
    3.AOP Client
    IProxy：动态代理注册标记接口(客户端代码类注册为动态代理标记)
    
Log目录
-------
### Elmah目录
基于Elmah的log封装<br/>

    ElmahResult：Elmah MVC Route实现
    
Media目录
-------
Video相关<br/>

    MediaInfoHelper：获取视频信息类，包括媒体信息、视频时长、文件大小、媒体格式、码率等
    
MVC目录
-------
### HtmlHelper目录
MVC Html辅助方法扩展<br/>

    ScriptBlockExtension：保证部分控件中的script位于指定位置，比如：body结束之前
    
### ResultExtension目录
MVC ActionResult扩展<br/>

    JsonpResult：以jsonp方式返回结果
    
OAuth目录
-------
基于dotnetopenauth的OAuth2方案实现<br/>

    TokenFilterAttribute：资源服务器认证过滤器
    
Repository目录
-------
### Infrastructure目录
仓储模式&工作单元基础架构<br/>

    IRepo：仓储接口
    IUnitOfWork：工作单元接口
    
### EntityFromwork目录
基于EntityFromwork的仓储模式&工作单元实现<br/>

    Repo<T>：基于entityfromwork的泛型仓储实现
    IEntityFromworkUnitOfWork：基于entityfromwork的工作单元实现
    
CacheHelper
--------
.net runtime cache帮助类
    
DESHelper
--------
DES3加密解密帮助类
    
JsonHelper
--------
Json帮助类
    
LinqHelper
--------
Linq扩展类
    
StringHelper
--------
字符串扩展帮助类
    
    
