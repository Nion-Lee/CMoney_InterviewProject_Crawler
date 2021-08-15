# CMoney_InterviewProject_Crawler  
此專案為CMoney全曜財經之面試考題，專案框架使用.NET 5、Console App。  
  
任務需求為對運動員網站進行爬蟲，解析個別生涯紀錄，並儲存成csv檔。  
架構借鑒了DDD、Clean Architecture等相關設計理念；並使用DI註冊服務，以及非同步串流以降低效能空耗。  
  
考題需求細節，請參閱檔案內之「考題需求-詳細文件.pdf」。  
Domain/Application/Infrastructure - IAsyncEnumerable<T>/await foreach  
