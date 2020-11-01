## Cloud computing:

- Routers know where to send your packages on the internet.
- Switch takes a package and send it to the correct server / client on your network.
- Scaling is limited in traditional architectures.
- Clound computing is the on demand delivery of compute power, database storage, applications and other IT resources.
- Pay as you go pricing.
- You can access as many resources as you need, almost instantly.
    - **Private cloud:** cloud services used by a single organization, not exposed to the public.
    - **Public cloud:** Azure, Google cloud and AWS.
        - Delivered over the internet.
    - **Hybrid cloud:**
        - Keep some services on premises and extend some others to the cloud.
- Five characteristics of cloud computing:
    1. On demand and self-service.
    2. Broad network access (available over the network and can be accessed by diverse client platforms).
    3. Multi-tenancy an resource pooling (multiple customers can share the same infrastructure and security with security and privacy). Multiple customers are serviced from the same physical resources.
    4. Rapid elasticity and scalability (automatically acquire and dispose resources as we need).
    5. Measured service.

- Six advantages:
    1. Trade capital expenses for operational expenses (you don't own hardware).
    2. Benefits by massive economies of scale (price reduced due to large scale).
    3. Stop guessing capacity.
    4. Increase speed and agility.
    5. Stop spending money running and maintaining data centers.
    6. Go global in minutes: leverage the AWS global infraestructure.
- Types of cloud computing:
    1. Infraestructure as a service (IaaS)
        - Building blocks for cloud IT
        - Networking, computers, data storage.
        - Amazon EC2
    2. Platform as a service (PaaS)
        - Removes the need of the organization to manage infraestructure.
        - Focus on the deployment and management of your applications.
        - Elastic Beanstack (on AWS).
    3. Software as a service
        - Organization doesn't manage anything.
        - Reckognition.
- Pricing:
    - Compute: pay for the exact compute time.
    - Storage: pay for the exact amount of data stored for the cloud.
    - Data transfer OUT the cloud (data transfer in is free).
- AWS Global Infraestructure:
    - AWS regions.
        - All around the world: Paris, Spain, etc.
        - Names: us-east1, eu-west-3, etc.
        - A cluster of data centers.
        - Most services will be linked in the scope to a specific region.
    - AWS availability zones.
        - One or more discrete data centers with redundant power, networking and connectivity.
        - ap-southeast-2a, ap-southeast-2b, ap-southeast-2c.
        - They are all connected with high bandwidth ultra low latency networking.
    - AWS Edge locations. 
        - 216 points of presence in 84 cities across 42 countries.
        - Content is delivered to end users with lower latency.
    - Region table to check availability of the service in your region.
    