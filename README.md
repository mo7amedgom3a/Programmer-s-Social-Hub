# Programmer's Social Hub

**Programmer's Social Hub** is a social media platform designed for programmers to share their knowledge, tips, tricks, and code snippets. Users can create and engage with posts, follow other users, comment on discussions, and interact in a vibrant community dedicated to programming and software development.

## Table of Contents
- [Features](#features)
- [Architecture](#architecture)
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
- **Image Uploads**: Support for image uploads with Cloudflare CDN integration.
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



## Technologies Used

### Frontend
- **Next.js**: React framework for server-side rendering and static site generation.

### Backend
- **.NET**: Backend framework for building the RESTful APIs and microservices.
- **MongoDB**: Non-relational database for managing unstructured data like posts and comments.
- **MySQL**: Relational database for handling structured data like user profiles.

### Microservices & Communication
- **RabbitMQ**: Message broker for handling asynchronous tasks and communication.
- **gRPC**: Protocol for fast inter-service communication.
- **Docker**: Containerization of services for consistent deployment.
- **Kubernetes**: Orchestrates containerized services for automated deployment and scaling.
- **Kong**: API Gateway for managing and securing APIs.

### Security
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

