create table "ShoppingCart"
(
	id integer not null
		constraint shoppingcart_pk
			primary key,
	"UserId" bigint not null
);

/* alter table "ShoppingCart" owner to cartapp; */

create index "ShoppingCart_UserId"
	on "ShoppingCart" ("UserId");

create table "ShoppingCartItems"
(
	id integer not null
		constraint shoppingcartitems_pk
			primary key,
	"ShoppingCartId" integer not null
		constraint "FK_ShoppingCart"
			references "ShoppingCart",
	"ProductCatalogId" bigint not null,
	"ProductName" varchar(100) not null,
	"ProductDescription" varchar(500),
	"Amount" integer not null,
	"Currency" varchar(5) not null
);

/* alter table "ShoppingCartItems" owner to cartapp; */

create index "ShoppingCartItems_ShoppingCartId"
	on "ShoppingCartItems" ("ShoppingCartId");

create table "EventStore"
(
	id integer not null
		constraint eventstore_pk
			primary key,
	"Name" varchar(100) not null,
	"OccurredAt" timestamp not null,
	"Content" varchar not null
);

/* alter table "EventStore" owner to cartapp; */
