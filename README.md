# CMoney_InterviewProject_Crawler  
此專案為CMoney全曜財經之面試考題，專案框架使用.NET 5、主控台專案。  
  
任務需求為對運動員網站進行爬蟲，解析個別生涯紀錄，並儲存成csv檔。  
內容使用到DDD、Clean Architecture等相關設計風格，使用DI註冊服務，並使用非同步串流降低效能空轉。  
  
Domain/Application/Repository；IAsyncEnumerable<T>/await foreach
