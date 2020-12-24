Steps run the Shopping Cart micro service
1. cd HelloMicroservices/HelloMicroservices
1. docker build -f Dockerfile -t micro:dev ..
1. docker run -d -p 8087:80 --name micro -e ASPNETCORE_ENVIRONMENT=Development micro:dev
