pipeline {
    agent any

    environment {
        DOCKER_IMAGE = "communitybackendapi:latest"
        APP_SERVER = "45.192.108.44"
        SSH_CREDENTIALS_ID = 'f3bbe46b-0002-467c-924e-acc00d699fa5'
    }

    stages {
        stage('Checkout Code') {
            steps {
                git credentialsId: 'f3bbe46b-0002-467c-924e-acc00d699fa5', url: 'https://github.com/eeee3e3e/CommunityBackendAPI.git'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test'
            }
        }

        stage('Deploy to Application Server') {
            steps {
                sshagent([SSH_CREDENTIALS_ID]) {
                    sh """
                        ssh root@${APP_SERVER} "
                            cd /path/to/deployment/directory &&
                            docker build -t ${DOCKER_IMAGE} . &&
                            docker stop communitybackendapi || true &&
                            docker rm communitybackendapi || true &&
                            docker run -d --name communitybackendapi -p 6000:6000 ${DOCKER_IMAGE}
                        "
                    """
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
