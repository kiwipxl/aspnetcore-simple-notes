-- Assuming we have an empty database created! (run database.sql)

\set ON_ERROR_STOP true

TRUNCATE TABLE note;
TRUNCATE TABLE label;
TRUNCATE TABLE image;

INSERT INTO label(user_id, name)
VALUES(1, 'Social');

INSERT INTO label(user_id, name)
VALUES(1, 'Gratitude');


INSERT INTO note(user_id, title, body, labels, created, edited)
VALUES(1, 'Museum visit', 'Visited the museum today! Very grateful.', 
	ARRAY[(SELECT id FROM label WHERE name = 'Social'), (SELECT id FROM label WHERE name = 'Gratitude')], 
	CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

	INSERT INTO image(user_id, note_id, data) VALUES
	(1, (SELECT currval('note_id_seq')), 'image1'),
	(1, (SELECT currval('note_id_seq')), 'image2');

	UPDATE note SET images = (SELECT ARRAY_AGG(id) from image WHERE note_id = currval('note_id_seq')) WHERE id = currval('note_id_seq');


INSERT INTO note(user_id, title, body, labels, created, edited)
VALUES(1, 'Gratitude 29/8', 'Im grateful for technology today.', 
	ARRAY[(SELECT id FROM label WHERE name = 'Gratitude')], 
	CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);


INSERT INTO note(user_id, title, body, labels, created, edited)
VALUES(1, 'Jeff', 'Lunch with Jeff today!', 
	ARRAY[(SELECT id FROM label WHERE name = 'Social')], 
	CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);


INSERT INTO note(user_id, title, body, labels, created, edited)
VALUES(1, 'Gratitude 28/8', 'Im grateful for sunshine today.', 
	ARRAY[(SELECT id FROM label WHERE name = 'Gratitude')], 
	CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
	
	INSERT INTO image(user_id, note_id, data) VALUES
	(1, (SELECT currval('note_id_seq')), 'image1');

	UPDATE note SET images = (SELECT ARRAY_AGG(id) from image WHERE note_id = currval('note_id_seq')) WHERE id = currval('note_id_seq');


-- Dump tables
SELECT * FROM note;
SELECT * FROM label;
SELECT * FROM image;

-- Get all notes with the 'Social' label
SELECT * FROM note WHERE (SELECT id FROM label WHERE name = 'Social') = ANY(labels);