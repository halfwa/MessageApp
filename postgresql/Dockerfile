
FROM postgres:16.1-alpine3.18

COPY postgresql/postgresql.conf /etc/postgresql.conf

CMD ["postgres", "-c", "config_file=/etc/postgresql.conf"]