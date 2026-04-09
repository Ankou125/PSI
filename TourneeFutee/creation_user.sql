DROP USER IF EXISTS 'test'@'localhost';
CREATE USER 'test'@'localhost' IDENTIFIED BY 'motdepasse';
GRANT ALL privileges 
ON tourneefutee.* TO 'test'@'localhost';
