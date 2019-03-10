CREATE TABLE [CMS].[Landing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Landing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--


CREATE TABLE [CMS].[LandingSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LandingSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingSettings]  WITH CHECK ADD  CONSTRAINT [FK_LandingSettings_Landing] FOREIGN KEY([LandingId])
REFERENCES [CMS].[Landing] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingSettings] CHECK CONSTRAINT [FK_LandingSettings_Landing]
GO--



CREATE TABLE [CMS].[LandingBlock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ContentHtml] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](255) NOT NULL,
	[Settings] [nvarchar](max) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_LandingPageBlock_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingBlock]  WITH CHECK ADD  CONSTRAINT [FK_LandingPageBlock_Landing] FOREIGN KEY([LandingId])
REFERENCES [CMS].[Landing] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingBlock] CHECK CONSTRAINT [FK_LandingPageBlock_Landing]
GO--




CREATE TABLE [CMS].[LandingSubBlock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingBlockId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ContentHtml] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](255) NOT NULL,
	[Settings] [nvarchar](max) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_LandingSubBlock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingSubBlock]  WITH CHECK ADD  CONSTRAINT [FK_LandingSubBlock_LandingBlock] FOREIGN KEY([LandingBlockId])
REFERENCES [CMS].[LandingBlock] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingSubBlock] CHECK CONSTRAINT [FK_LandingSubBlock_LandingBlock]
GO--










