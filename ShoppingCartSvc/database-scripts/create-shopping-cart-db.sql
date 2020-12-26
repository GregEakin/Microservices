create table "ShoppingCart"
(
	id serial not null
		constraint shoppingcart_pk
			primary key,
	"UserId" int not null
		constraint userid_key
			unique
);

create index "ShoppingCart_UserId"
	on "ShoppingCart" ("UserId");

/* alter table "ShoppingCart" owner to cartapp; */

create table "ShoppingCartItems"
(
	id serial not null
		constraint shoppingcartitems_pk
			primary key,
	"ShoppingCartId" int not null
		constraint "FK_ShoppingCart"
			references "ShoppingCart",
	"ProductCatalogId" int not null,
	"ProductName" varchar(100) not null,
	"ProductDescription" varchar(500),
	"Amount" money not null,
	"Currency" varchar(5) not null
);

/* alter table "ShoppingCartItems" owner to cartapp; */

create index "ShoppingCartItems_ShoppingCartId"
	on "ShoppingCartItems" ("ShoppingCartId");

create table "EventStore"
(
	id serial not null
		constraint eventstore_pk
			primary key,
	"Name" varchar(100) not null,
	"OccurredAt" timestamp not null,
	"Content" varchar not null
);

/* alter table "EventStore" owner to cartapp; */

