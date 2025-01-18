pipeline {
    agent any

    environment {
        DOCKER_IMAGE = "communitybackendapi:latest"
        APP_SERVER = "38.55.235.162"
        SSH_CREDENTIALS_ID = 'b8b640ea-cf83-48cf-a16b-b71ec8391baa'
    }

    stages {
        stage('Checkout Code') {
            steps {
                git credentialsId: 'b8b640ea-cf83-48cf-a16b-b71ec8391baa', url: 'https://github.com/eeee3e3e/CommunityBackendAPI.git'
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
                sshagent(['[SSH_CREDENTIALS_ID]']) {
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
