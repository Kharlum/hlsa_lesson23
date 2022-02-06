1. setup 2 instance of EC2 (AWS)  
1.1. install docker  
1.2. push image with asp.net core api  
1.3. run docker container with asp.net core api  
![C10](screens/ec2_instances.jpg)  

2. setup classic load balancer  
![C10](screens/balancer_description.jpg)
![C10](screens/balancer_instances.jpg)
![C10](screens/balancer_listeners.jpg)
![C10](screens/balancer_health_check.jpg)  

3. setup 2 security group (AWS)  
![C10](screens/sg_api.jpg)
![C10](screens/sg_balancer.jpg)  

4. run siege test  
5. check monitoring (AWS)  
![C10](screens/traffic.jpg)
![C10](screens/traffic_balancer.jpg)  

For check load balancer work:  
```
siege -c60 -t60S http://{dns_domain_load_balancer}/api/health/healthy 
```
