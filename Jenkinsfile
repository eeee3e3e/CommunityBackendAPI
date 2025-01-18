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

        stage('Docker Build') {
            steps {
                sh "docker build -t ${DOCKER_IMAGE} ."
            }
        }

        stage('Docker Push') {
            steps {
                withDockerRegistry(credentialsId: 'b8b640ea-cf83-48cf-a16b-b71ec8391baa', url: 'https://your-docker-registry') {
                    sh "docker tag ${DOCKER_IMAGE} your-docker-registry/${DOCKER_IMAGE}"
                    sh "docker push your-docker-registry/${DOCKER_IMAGE}"
                }
            }
        }

        stage('Deploy to Application Server') {
            steps {
                sshagent(['${SSH_CREDENTIALS_ID}']) {
                    sh """
                        ssh root@${APP_SERVER} "docker pull your-docker-registry/${DOCKER_IMAGE} &&
                                                docker stop communitybackendapi || true &&
                                                docker rm communitybackendapi || true &&
                                                docker run -d --name communitybackendapi -p 6000:6000 your-docker-registry/${DOCKER_IMAGE}"
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
