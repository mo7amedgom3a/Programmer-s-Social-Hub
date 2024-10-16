class user_management_service {

    # Ensure .NET is installed
    package { 'dotnet-sdk-8.0':
        ensure => installed,
    }

    # Ensure Docker is installed
    package { 'docker':
        ensure => installed,
    }

    # Ensure Docker service is running
    service { 'docker':
        ensure => running,
        enable => true,
        require => Package['docker'],
    }

    # Ensure MongoDB is installed
    package { 'mongodb-org':
        ensure => installed,
    }

    # Ensure MongoDB service is running
    service { 'mongod':
        ensure => running,
        enable => true,
        require => Package['mongodb-org'],
    }
}

include user_management_service