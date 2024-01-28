docker build -t d-flat-chain .
docker run --name dflat-chain -p 80:80 d-flat-chain

@REM docker run -p 80:80 d-flat-chain