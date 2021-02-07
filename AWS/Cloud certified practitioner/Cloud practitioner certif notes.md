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
        - Shared responsability model. Customer is responsable for the security in the cloud (configuration, etc). AWS is responsable for the security of the cloud.
        - Customer is responsible for operating system updates and patches of the instances.

    
    - **IAM:** identity access management.
        - It is a global service (doesn't need a region).
        - Since it is a global service when we create users they are for all regions.
        - Create users and assign them to group.
        - Root account created by default. 
        - One user represents one person in the organization.
        - Groups can only contain users not other groups.
        - Not all users have to belong to a group.
        - A user can belong to multiple groups.
        - User / groups can be assigned JSON documents called policies. These policies helps us to define permissions for our users.
        - IAM -> Users -> Add user -> Add access to the AWS console -> And password -> Add user to a group -> Create group -> Attach policy to the group (admins) -> Create group. 
        - -> Tags
            - Tags are a way to mark the users and add them some attributes but they don't change how AWS works.
        - -> Review and create user.
        - You can download a csv with the user information. 
        - The policy has an effect (Allow), an action (get*, list*) and a resource (*) to grant access to the users to different actions in certain resources.
        - You can create your custom policy usinga visual editor or creation the JSON file.
        - Password policy: 
            - Minimum length.
            - Require specific character types.
            - Allow users to change their passwords.
            - Require your users to change the password after some time.
            - Prevent password re use.
            - To change the policy: IAM -> Account settings -> Set password policy -> Change it and save changes.
        - Multi factor authentication (MFA device).
            - Virtual MFA device (google authenticaticator or Authy).
            - Universal 2nd factor (U2F) Security Key (USB) (Yubikey).
            - In the Dashboard of the the root user -> Security Alerts (Enable MFA) ->  Install of compatible application (Authy) -> Scan del QR code -> Write two consecutive 
    - Three ways to access AWS:
        1. AWS management console.
        2. AWS Command Line Interface (CLI).
            - Install AWS CLI on windows (download version 2 for windows).
            - Open CMD and type:
            
            ```batch
            aws --version
            ``` 
        3. AWS Software Developer Kit (SDK). For this you need to generate access keys through AWS Console.
            -  Access Key ID : user name
            - Secret Access Key : password
            - To create an access key login with an account user (not root).
                - IAM -> Users => Select user => Security Credentials Tab => Create Access Key 
                - aws configure
                    - Insert AWS Key Id.
                    - Insert AWS Secret Access Key.
                    - Default region name.
                    - Default output format (Enter).
                    ```batch
                    aws iam list-users
                    ```
            - AWS cloud shell (a terminal in the cloud inside AWS portal). 
            - IAM roles for services
                - Some AWS service will need to perform actions on your behalf (EC2 instance virtual server).
                - To do so we will assign permissions to AWS services with IAM roles.
                - EC2 instance roles.
                - Lamda function roles.
                - Roles for CloudFormation.
                IAM -> Roles -> Create Role -> AWS service -> EC2 -> Next: permissions -> IAMReadOnlyAccess 
                    -> Tags -> Review (name) => Create.
        - IAM Security tools.
            - **IAM Credentials Report:** (account-level), which is a report that lists all your account's users and the status of their various credentials.
                IAM -> Credential report ->  Download.
                - when the user created, password policies, access_key_1_active.
            - **IAM Access Advisor:** (user-level), shows the service permissions granted to a user and when those where last accessed.
                IAM -> Users -> Select a user -> Access advisor.
                    - It shows which last services where used.
        - IAM best practices:
            - Don't use root account.
            - One physical user = one aws user.
            - Assign users to groups and assign permissions to grupos (manage permissions at group level not user level).
            - Create a strong password policy.
            - Use and enforce the user of MFA.
            - Use Access Keys for programmatic access (CLI / SDK).
            - Audit permissions of your account with the IAM credentials report.
            - Never share IAM users and access keys.
        - **Shared responsability model for IAM:**
            - AWS is responsabie for:
                - Infraestructure
                - Configuration and vulnerability analysis.
                - Compliance validation.
            - You
                - Users, groups, roles policies management and monitoring.
                - Enable MFA on all accounts.
                - Rotate all your keys often.
                - Use IAM tools to apply appropiate permisssions.
                - Analyze access patterns and review permissions.
        - You can create a budget in the billing section. 
            Billing -> Create budget -> Cost budget -> Recurring -> 10 dollars monthly -> Configure threshold 
    
    - **EC2 Section:**
        - Elastic Compute Cloud, infraestructure as a service.
            - You can rent VM.
            - You can store data on virtual drives (EBS).
            - Distribute load accross machines (ELB).
            - Scale services using auto scaling group (ASG).
        -  Configuration options:
            - OS (Windows and Linux).
            - Compute power access (CPU).
            - Random access memory.
            - Storage space:
                - Network attached (EBS & EFS).
                - Hardware (EC2 Instance Store).
            - Network card.
            - Firewall rules.
            - Bootstrap script (configure at first launch).
        - EC2 -> Instances -> Launch instance -> Choose amazon AMI (template) -> Choose an instance type based on your needs -> Configure instance details -> User data text (copy script to configure a web service) -> Add storage -> Add tags -> Configure security group (firewall) (Add rule -> http -> port:80 -> from 0.0.0.0/0 (everywhere)) -> Review and launch -> Create new key pair.
        - Once finished launching it gives you a public ip address to watch the website.
        - Once you stop your instance you don't have to pay for it.
        - You can also terminate your instance (delete it).
        - If you stop and start the instance you get a new public ip.
        - Instance types: https://aws.amazon.com/ec2/instance-types
            - m5.2large:
                - m: instance class
                - 5: generation (AWS improves them over time).
                - 2xlarge: size within the instance class.
            - Different types:
             1. **General purpose:** 
                - Balance between compute, memory and networking.
             2. **Compute optimized:** 
                - High performance processors. (C name)
                - For batch processing workloads, media transcoding, machine learning, gaming servers, etc.
             3. **Memory optimized:**
                - R name
                - Fast performance for workloads that process large data sets in memory.
                -   Floating point number calculations, data pattern matching, graphic processing.
             4. **Storage optimized:**
                - Name i, h, d.
                - Online transaction processing.
                - Relational and NoSQL.
                - Data warehousing applications.
                - Distributed file systems.

        - Security groups:
            - They control how traffic is allowd into or out of the EC2 instances.
            - They only contain allow rules.
            - Can reference by IP or by security group.
            - They act as a firewall on EC2 instances.
            - Are going to regulate access to ports, authorized ip ranges, control the inbound network (from the other to the instance) and outbound network (from the instance to the other):
                - SSH port 22. (Log into a Linux instance)
                - FTP port 21. (upload files into a file share)
                - SFTP port 22. (upload files using SSH)
                - HTTP port 80 (access unsecured websites). 
                - HTTPS port 443 (access secured websites).
                - RDP port 3389 (Remote Desktop Protocol, log into a Windows instance).
            - You can go to security groups and edit inbound and outbound rules.
            - You can reuse security groups in multiple instances.
            - Anywhere IP v4: 0.0.0.0/0. Anywhere IP v6: ::/0.
    - Connect to our instance using **ssh**:
    - Use the Public IP to connect using SSH.
    - Use the key file (EC2Tutorial.pem) to get into the machine.
    - chmod 0400 for the key file (only read permissions). 
    - To access the EC2 instance via CLI:
    ```batch
     ssh -i .\EC2Tutorial.pem ec2-user@18.223.114.120
    ```
    - For windows versions < 10 you need to use Putty instead of SSH.
    - The user is always ec2-user.
    - For windows the file must have full access and only your user should have access to it (disable inheritance).
    - **EC2 instance connect:** Instances -> Select instance -> Connect -> opens an ssh terminal in the browser (aws will automatically upload the key to the instance).
    - This doesn't work if you block the ssh port.
    - **Amazon instance roles:**
        - Don't use your AWS credentials on the instance to execute AWS commands, instead use instance roles.
        - Attach role to instance: Select the instance => Security => Modify IAM role => DemoRoleForEC2.
        ```batch
        aws iam list-users
        ```
    - **EC2 Instance Launch types**
        - EC2 On demand:
            - Billing per second after the first minute.
            - Has the hightest cost but not upfront payment.
            - No long-term commitment.
        - EC2 Reserved instance (minimum of 1 year, 1 or three not between 1 and three).
            - Up to 75 % discount compared to on-demand.
            - Reserve a specific instance type.
        - Convertible reserved (long workloads).
            - Can change the EC2 instance type.
            - Up to 54 % discount.
        - Scheduled reserved instances.
            - Specific time window (fraction of day).
        - EC2 Spot instances
            - Discount of 90 % compared to On-demand but you can lose them if your max price is less than the current spot price.
            - Not for critical job.
        - For workloads that are resilient to failure (batch jobs, image proccessing). 
        - Dedicated hosts. 
            - Physical server in a data center.
            - Compliance requirementes and reduce costs by allowing you to user your existing server-bound software licenses. 
            - Allocated for your account for a 3 year period reservation.
            - More expensive. 
            - Per host billing. 
        - Dedicated instances (hardware dedicated to you).
            - May share hardware with other instances in the same account.
            - No control over instance placement.
            - Per instance billing.
    - **EBS Volume (Elastic Block Store)**
        - Network drive that you can attach to your instances while they run.
        - Persist data even after the instance termination.
        - Can only be mounted to one instance at a time.
        - Bound to a specific availability zone.
        - Network USB stick.
        - 30 GB of free EBS storage of type gp2 per month.
        - It's not a physical drive.
        - Can be detached from an EC2 instance and atached to another one quickly.
        - It's locked to an Availability Zone (AZ).
        - To move volume across you first need to snapshot it.
        - Have a provisioned capacity in advance (size in GBs).
        - You can increase capacity over time.
        - Can have more than one EBS attached to the same instance.
        - It is possible to create an EBS volume and leave them unatached.
        - Instance details root device and block devices (EBS volumes).
        - A volume is created when you create your EC2 instance.
        - EC2 -> Elastic Block Store -> Volumes -> Create volume -> gp2 -> change size -> availability zone must be in the same zone as your instance -> create volume.
        - Select the volume -> actions -> Attach volume -> select your instance. 
        - To check it Go to instances -> select your instance -> Select the Storage tab.
        - ESB volumes that do not have delete in termination set to true will persist even if you delete the instance.
        - EBS snapshots: you can make snapshots of your volumes and copy them across AZ or regions.
            - Select Volume -> Actions -> Create snapshot
            - To verify the goto to EBS -> Snapshots (under volumes).
            - The snapshot is available in your region, not to specific AZ (availability zone).
            - You can create a volume from a snapshot
                - Select snapshot -> Actions -> Create volume. You can configure it in a different availability zone.
    - **AMI (Amazon Machine Image)**
        - A customization of an EC2 instance.
        - It can be built for an specific region.
        - You can add your own software, configuration, operation system, monitoring, etc.
        - You have to make it and mantain it yourself or launch (and buy) from AWS marketplace.
        - AMI process:
            - Start an EC2 instance and customize it.
            - Stop the instance (for data integrity).
            - Build an AMI.
            - Launch instances from other AMIs.
        - Right click in the instance you want -> Image and templates -> Create image -> Type name (it will create a snapshot of the root volume) -> Create
        - Now in Launch instance it will appear under the MyAMIs tab.
        - If you create a new instance for yoIr AMI you will have the web server configured without needing to paste the configuration script (but it will have the same private ip address)
    - **EC2 instance store**
        - If you need high performance hardware disk.
        - Better IO performance.
        - They loose their storage when they're stopped.
        - Risk of data loss if hardware fails.
    - **EFL Elastic File System**
        - Can be mounted in hundreds of EC2 instances at a time.
        - IT works with Linux EC2 instances in multi-AZ.
        - 
        