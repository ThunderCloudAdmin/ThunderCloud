# Configuration file for the Sphinx documentation builder.

# -- Project information

project = 'Thundercloud'
copyright = '2024, Zenkraker'
author = 'Zenkraker'
release = '0.0.1'

# -- General configuration

extensions = [
    'sphinx.ext.duration',
    'sphinx.ext.doctest',
    'sphinx.ext.autodoc',
    'sphinx.ext.autosummary',
    'sphinx.ext.intersphinx',
    'myst_parser'
]

intersphinx_mapping = {
    'python': ('https://docs.python.org/3/', None),
    'sphinx': ('https://www.sphinx-doc.org/en/master/', None),
}
intersphinx_disabled_domains = ['std']

templates_path = ['_templates']

# -- Options for HTML output

html_theme = 'cloud'
html_theme_options = {
    'logo_only': True,
    'display_version': False,
    'description': 'A cloud storage solution but... self-hosted',
    'github_repo': 'https://github.com/ThunderCloudAdmin/ThunderCloud'
}
#html_logo = 'img/logo.png'

# -- Options for EPUB output
epub_show_urls = 'footnote'
