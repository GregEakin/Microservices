Steps run the Shopping Cart micro service
1. docker build -f ShoppingCartSvc/Dockerfile -t micro:dev .
1. docker run -d -p 8087:80 --name micro -e ASPNETCORE_ENVIRONMENT=Development micro:dev
