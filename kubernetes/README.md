# Kubernetes (k8s)

## kubectl commands
+ `kubectl version`
+ `kubectl cluster-info`
+ `kubectl get all`
+ `kubectl run [container-name] --image=[image]`
+ `kubectl port-forward [pod] [ports]`
+ `kubectl expose ...`
+ `kubectl create [resource]`
+ `kubectl apply [resource]`
+ `kubectl logs ...`

## K8S UI dashboard
+ `kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml`
+ Dashboard URL: http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/