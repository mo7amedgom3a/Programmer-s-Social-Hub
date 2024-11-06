# Programmer's Social Hub

**Programmer's Social Hub** is a social media platform designed for programmers to share their knowledge, tips, tricks, and code snippets. Users can create and engage with posts, follow other users, comment on discussions, and interact in a vibrant community dedicated to programming and software development.

## Demo

Check out our demo video and presentation slides to get a better understanding of **Programmer's Social Hub**:

- [Demo Video](https://drive.google.com/file/d/1opBmqzF2yvWJkeiwA9C643NJga5oIEPY/view?usp=sharing)
- [Presentation Slides](https://docs.google.com/presentation/d/18LxqoeZ94WPqW7SCQ5xTNJu0yZcw9NrqghdhBc2cz6k/edit?usp=sharing)


## Technology Used
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![gRPC](https://img.shields.io/badge/gRPC-4285F4?style=for-the-badge&logo=grpc&logoColor=white)
![Kong](https://img.shields.io/badge/Kong-00ADEF?style=for-the-badge&logo=kong&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=signalr&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Kubernetes](https://img.shields.io/badge/Kubernetes-326CE5?style=for-the-badge&logo=kubernetes&logoColor=white)
![Jenkins](https://img.shields.io/badge/Jenkins-D24939?style=for-the-badge&logo=jenkins&logoColor=white)
![Puppet](https://img.shields.io/badge/Puppet-FFAE1A?style=for-the-badge&logo=puppet&logoColor=white)



## Table of Contents
- [Features](#features)
- [Architecture](#architecture)
    - [Overview Architecture](#overview-architecture)
    - [Detailed Architecture](#detailed-architecture)
        - [Security Service](#security-service)
        - [User Management Service](#user-management-service)
        - [Post Service](#post-service)
        - [Notification Service](#notification-service)
        - [Search and Filter Service](#search-and-filter-service)
        - [API Gateway (Kong)](#api-gateway-kong)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Microservices](#microservices)
- [Security](#security)
- [Contributing](#contributing)
- [License](#license)

## Features

- **User Profiles**: Create and manage developer profiles with features to follow/unfollow other users, showcase skills, and highlight areas of interest.
- **Posts and Code Sharing**: Share programming tips, tricks, tutorials, errors, and code snippets using a built-in code editor.
- **Real-time Notifications**: Get real-time updates on comments, reactions, and followers.
- **Image Uploads**: Support for image uploads with cloudinary CDN integration.
- **Search**: Powerful search functionality with topic and profile suggestions.
- **Microservices Architecture**: Scalable architecture with independent services for security, user management, posts, notifications, and more.

## Architecture

The system is built using a microservices architecture, allowing each component to scale independently and communicate efficiently. The key components include:

- **Frontend**: Developed using **Next.js** for server-side rendering and fast client-side interactivity.
- **Backend**: Built with **.NET** using a microservices approach to ensure modularity and scalability.
- **Databases**: Uses **MySQL** for relational data and **MongoDB** for non-relational data storage.
- **Message Queue**: Integrates **RabbitMQ** for asynchronous communication between services.
- **gRPC**: Used for efficient inter-service communication between microservices.
- **API Gateway**: Managed by **Kong** to secure and manage the APIs.

## Overview Architecture

The architecture of **Programmer's Social Hub** is designed to be scalable, resilient, and efficient, leveraging modern technologies and best practices. Below is a high-level overview of the system architecture:

![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/protofolio.jpg?raw=true)
## Detaild Architecture
### Security Service
The Security Service is responsible for authenticating users and ensuring that all interactions on the platform are secure. It uses JSON Web Tokens (JWT) for user authentication and authorization, with asymmetric key pairs to sign and verify tokens.

- **Key Features**

1)  User registration and login.
2)  Token-based authentication with JWT.
3)  Secure data exchange using asymmetric encryption.
![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/Security%20Service.drawio.png?raw=true)

### **User Management Service**
The User Management Service handles all operations related to user profiles and their interactions with other users. It is built using .NET and MongoDB to store user data efficiently.
- **Key Features**
1) Create and manage user profiles.
2) Follow and unfollow other users.
![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/UseerService.drawio.png?raw=true)
### **Post Service**
The Post Service is where users can create, edit, and delete their posts. This service also handles comments, reactions (likes, dislikes), and saved posts. It uses MongoDB to store post-related data, making it easy to manage and retrieve.
- **Key Features**:

1) Create and manage posts, comments, and reactions.
2) Save favorite posts for later viewing.
3) Integrated code editor for sharing code snippets for Posts and Comments.
![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/PostService.drawio.png?raw=true)

### **Notification Service**
The Notification Service is designed to provide real-time updates to users using SignalR in .NET Core. It notifies users about new followers, comments, reactions, and other activities happening on their posts.
- **Key Features**:

1) Real-time push notifications for various user activities by using Signalr.
2) Integration with RabbitMQ for asynchronous event handling.
3) gRPC communication with other services for user data retrieval.
![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/NotificationService.drawio.png?raw=true)

### **Search and Filter Service**
The Search and Filter Service provides a powerful search engine that allows users to find posts, topics, and profiles quickly.
- **Key Features**:

1) Full-text search for posts, topics, and profiles.
3) Filtering options to refine search results.
![Overview Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/NotificationService.drawio.png?raw=true)

### **API Gateway (Kong)**
The API Gateway uses Kong to manage and secure all incoming API requests. It acts as a reverse proxy, routing client requests to the appropriate microservices and ensuring secure communication.
- **Key Features**:

1) Centralized API management and traffic control.
2) Security and rate-limiting for APIs.
3) Load balancing and request routing.



### Frontend
- **Next.js**: React framework for server-side rendering and static site generation.


### Backend
- **.NET**: Backend framework for building the RESTful APIs and microservices.
![.NET Architecture](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/.netArchitecture.png?raw=true)
- **MongoDB**: Non-relational database for managing unstructured data like posts and comments. The following collections are used:

    - **User Collection**:
    
        ![User Collection](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/UserServiceCollections.png?raw=true)

    - **Post Collection**:
        ![Post Collection](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/PosCollection.png?raw=true)

    - **Comment Collection**:

        ![Comment Collection](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/CommentCollection.png?raw=true)

    - **Likes Collection**:

        ![Likes Collection](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/LikesCollection.png?raw=true)
- **MySQL**: Relational database for handling structured data like user profiles.
- **Security database**:
![MySQL Database](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/SecurityServiceDB.png?raw=true)

### Microservices & Communication
- **RabbitMQ**: Message broker for handling asynchronous tasks and communication.
- **gRPC**: Protocol for fast inter-service communication.
- **Docker**: Containerization of services for consistent deployment.
- **Kubernetes**: Orchestrates containerized services for automated deployment and scaling.
- **Kong**: API Gateway for managing and securing APIs.

### Security
![auth](https://github.com/mo7amedgom3a/Programmers-Social-Hub/blob/main/images/auth-microservices.jpg?raw=true)
- **JWT (JSON Web Token)**: For authentication and authorization of users.
- **Asymmetric Keys**: Uses public/private keys for secure data exchange and JWT signing.

## Getting Started

### Prerequisites
- **Docker** and **Docker Compose**
- **Kubernetes** for orchestration
- **.NET SDK**
- **Node.js** and **npm/yarn** for the frontend
- **MySQL** and **MongoDB** for databases
- **RabbitMQ** for message queue setup

### Installation

1. **Clone the Repository**
    ```bash
    git clone https://github.com/your-username/programmers-social-hub.git
    cd programmers-social-hub
    ```

2. **Set Up Environment Variables**
    Create a `.env` file in the root directory and add the necessary environment variables as specified in `.env.example`.

3. **Start Services with Docker Compose**
    ```bash
    docker-compose up -d
    ```

4. **Apply Kubernetes Configurations**
    ```bash
    kubectl apply -f k8s/
    ```

5. **Run Database Migrations**
    ```bash
    dotnet ef database update
    ```

6. **Install Frontend Dependencies**
    ```bash
    cd frontend
    npm install
    npm run dev
    ```

## Microservices

The application is divided into several microservices, each responsible for a specific domain:

- **User Service**: Manages user profiles and authentication.
- **Post Service**: Handles creation, editing, and deletion of posts.
- **Notification Service**: Manages real-time notifications.
- **Search Service**: Provides search functionality across posts and profiles.

## Security

Security is a top priority, and the application employs several measures to ensure data protection:

- **JWT Authentication**: Secure user authentication and authorization.
- **Data Encryption**: Sensitive data is encrypted using industry-standard algorithms.
- **Rate Limiting**: Protects against abuse by limiting the number of requests a user can make.

## Contributing

We welcome contributions from the community! To contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add new feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

